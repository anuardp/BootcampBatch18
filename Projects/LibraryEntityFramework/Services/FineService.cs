using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services;
public class FineService
{
    private readonly LibraryDbContext _context;

    public FineService(LibraryDbContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<Fine> CreateFineAsync(int borrowId, decimal totalFine)
    {
        // Cek apakah sudah ada fine untuk borrow ini
        var existing = await _context.Fines.FirstOrDefaultAsync(f => f.BorrowId == borrowId);
        if (existing != null)
            throw new InvalidOperationException("Fine already exists for this borrow.");

        var fine = new Fine
        {
            BorrowId = borrowId,
            TotalFine = totalFine,
            HasPayFine = false,
            PaidDate = null
        };

        _context.Fines.Add(fine);
        await _context.SaveChangesAsync();
        return fine;
    }

    // READ
    public async Task<List<Fine>> GetAllAsync()
    {
        return await _context.Fines
            .Include(f => f.BorrowBook)
                .ThenInclude(bb => bb!.Visitor)
            .Include(f => f.BorrowBook)
                .ThenInclude(bb => bb!.BookCopy)
                .ThenInclude(bc => bc!.Book)
            .ToListAsync();
    }

    public async Task<Fine?> GetByIdAsync(int id)
    {
        return await _context.Fines
            .Include(f => f.BorrowBook)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Fine?> GetByBorrowIdAsync(int borrowId)
    {
        return await _context.Fines.FirstOrDefaultAsync(f => f.BorrowId == borrowId);
    }

    public async Task<List<Fine>> GetUnpaidFinesAsync()
    {
        return await _context.Fines
            .Where(f => !f.HasPayFine)
            .Include(f => f.BorrowBook)
                .ThenInclude(bb => bb!.Visitor)
            .ToListAsync();
    }

    public async Task<decimal> GetOutstandingFinesByVisitorAsync(int visitorId)
    {
        var total = await _context.Fines
            .Where(f => f.BorrowBook != null && f.BorrowBook.VisitorId == visitorId && !f.HasPayFine)
            .SumAsync(f => f.TotalFine);
        return total;
    }

    // UPDATE (pembayaran)
    public async Task<bool> PayFineAsync(int fineId, DateTime? paidDate = null)
    {
        var fine = await _context.Fines.FindAsync(fineId);
        if (fine == null) return false;
        if (fine.HasPayFine) return false; // sudah dibayar

        fine.HasPayFine = true;
        fine.PaidDate = paidDate ?? DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    // DELETE (jarang, hanya untuk keperluan koreksi)
    public async Task<bool> DeleteAsync(int id)
    {
        var fine = await _context.Fines.FindAsync(id);
        if (fine == null) return false;

        _context.Fines.Remove(fine);
        await _context.SaveChangesAsync();
        return true;
    }
}