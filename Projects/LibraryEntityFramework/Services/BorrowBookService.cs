using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services;
public class BorrowBookService
{
    private readonly LibraryDbContext _context;
    private readonly FineService _fineService;

    public BorrowBookService(LibraryDbContext context, FineService fineService)
    {
        _context = context;
        _fineService = fineService;
    }

    //CREATE
    public async Task<BorrowBook> BorrowAsync(int bookCopyId, int visitorId, int loanDurationDays = 14)
    {
        var visitor = await _context.Visitors.FindAsync(visitorId);
        if (visitor == null)
            throw new ArgumentException("Visitor not found.");

        var copy = await _context.BookCopies.FindAsync(bookCopyId);
        if (copy == null)
            throw new ArgumentException("Book copy not found.");
        if (!copy.IsAvailable)
            throw new InvalidOperationException("Book copy is not available for borrowing.");

        var outstandingFine = await _fineService.GetOutstandingFinesByVisitorAsync(visitorId);
        if (outstandingFine > 0)
            throw new InvalidOperationException($"Visitor has outstanding fines of {outstandingFine}. Please pay first.");

        var borrow = new BorrowBook
        {
            BookCopyId = bookCopyId,
            VisitorId = visitorId,
            BorrowBookStart = DateTime.UtcNow,
            BorrowBookDue = DateTime.UtcNow.AddDays(loanDurationDays),
            ReturnedDate = null
        };

        copy.IsAvailable = false;

        _context.BorrowBooks.Add(borrow);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(borrow.Id) ?? borrow;
    }

    // READ
    public async Task<List<BorrowBook>> GetAllAsync()
    {
        return await _context.BorrowBooks
            .Include(bb => bb.BookCopy).ThenInclude(bc => bc!.Book)
            .Include(bb => bb.Visitor)
            .Include(bb => bb.Fine)
            .OrderByDescending(bb => bb.BorrowBookStart)
            .ToListAsync();
    }

    public async Task<BorrowBook?> GetByIdAsync(int id)
    {
        return await _context.BorrowBooks
            .Include(bb => bb.BookCopy).ThenInclude(bc => bc!.Book)
            .Include(bb => bb.Visitor)
            .Include(bb => bb.Fine)
            .FirstOrDefaultAsync(bb => bb.Id == id);
    }

    public async Task<List<BorrowBook>> GetActiveBorrowsAsync()
    {
        return await _context.BorrowBooks
            .Where(bb => bb.ReturnedDate == null)
            .Include(bb => bb.BookCopy).ThenInclude(bc => bc!.Book)
            .Include(bb => bb.Visitor)
            .Include(bb => bb.Fine)
            .ToListAsync();
    }

    public async Task<List<BorrowBook>> GetBorrowHistoryByVisitorAsync(int visitorId)
    {
        return await _context.BorrowBooks
            .Where(bb => bb.VisitorId == visitorId)
            .Include(bb => bb.BookCopy).ThenInclude(bc => bc!.Book)
            .Include(bb => bb.Fine)
            .OrderByDescending(bb => bb.BorrowBookStart)
            .ToListAsync();
    }

    public async Task<List<BorrowBook>> GetOverdueBorrowsAsync()
    {
        return await _context.BorrowBooks
            .Where(bb => bb.ReturnedDate == null && bb.BorrowBookDue < DateTime.UtcNow)
            .Include(bb => bb.BookCopy).ThenInclude(bc => bc!.Book)
            .Include(bb => bb.Visitor)
            .ToListAsync();
    }

    // UPDATE
    public async Task<BorrowBook?> ReturnBookAsync(int borrowId, decimal? manuallyEnteredFine = null)
    {
        var borrow = await _context.BorrowBooks
            .Include(bb => bb.BookCopy)
            .FirstOrDefaultAsync(bb => bb.Id == borrowId);
        if (borrow == null) return null;
        if (borrow.ReturnedDate != null)
            throw new InvalidOperationException("Book already returned.");

        borrow.ReturnedDate = DateTime.UtcNow;

        // Hitung denda jika terlambat
        decimal fineAmount = 0;
        if (borrow.ReturnedDate > borrow.BorrowBookDue)
        {
            var daysLate = (borrow.ReturnedDate.Value.Date - borrow.BorrowBookDue.Date).Days;
            const decimal dailyFine = 1000; // atau ambil dari konfigurasi
            fineAmount = daysLate * dailyFine;
        }

        if (manuallyEnteredFine.HasValue)
            fineAmount = manuallyEnteredFine.Value;

        if (fineAmount > 0)
        {
            await _fineService.CreateFineAsync(borrow.Id, fineAmount);
        }

        // Update status copy menjadi tersedia
        if (borrow.BookCopy != null)
        {
            borrow.BookCopy.IsAvailable = true;
        }

        await _context.SaveChangesAsync();
        return await GetByIdAsync(borrow.Id);
    }

   
    public async Task<BorrowBook?> ExtendDueDateAsync(int borrowId, int additionalDays)
    {
        var borrow = await _context.BorrowBooks.FindAsync(borrowId);
        if (borrow == null) return null;
        if (borrow.ReturnedDate != null)
            throw new InvalidOperationException("Cannot extend due date of returned book.");
        if (borrow.BorrowBookDue < DateTime.UtcNow)
            throw new InvalidOperationException("Cannot extend overdue borrow. Please return and pay fine.");

        borrow.BorrowBookDue = borrow.BorrowBookDue.AddDays(additionalDays);
        await _context.SaveChangesAsync();
        return borrow;
    }

    // DELETE 
    public async Task<bool> DeleteAsync(int id)
    {
        var borrow = await _context.BorrowBooks.FindAsync(id);
        if (borrow == null) return false;

        _context.BorrowBooks.Remove(borrow);
        await _context.SaveChangesAsync();
        return true;
    }


}