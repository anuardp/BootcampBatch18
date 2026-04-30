using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services;
public class BookCopyService
{
    private readonly LibraryDbContext _context;

    public BookCopyService(LibraryDbContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<BookCopy> CreateAsync(BookCopy bookCopy)
    {
        // Pastikan BookId valid
        var bookExists = await _context.Books.AnyAsync(b => b.Id == bookCopy.Id);
        if (!bookExists)
            throw new ArgumentException("Invalid BookId.");

        // Set default jika perlu
        if (string.IsNullOrWhiteSpace(bookCopy.BookCode))
            bookCopy.BookCode = Guid.NewGuid().ToString().Substring(0, 8);
        bookCopy.IsAvailable = true; // salinan baru selalu tersedia

        _context.BookCopies.Add(bookCopy);
        await _context.SaveChangesAsync();
        return bookCopy;
    }

    // READ
    public async Task<List<BookCopy>> GetAllAsync()
    {
        return await _context.BookCopies
            .Include(bc => bc.Book)
            .ToListAsync();
    }

    public async Task<BookCopy?> GetByIdAsync(int id)
    {
        return await _context.BookCopies
            .Include(bc => bc.Book)
            .FirstOrDefaultAsync(bc => bc.Id == id);
    }

    public async Task<List<BookCopy>> GetByBookIdAsync(int bookId)
    {
        return await _context.BookCopies
            .Where(bc => bc.Id == bookId)
            .Include(bc => bc.Book)
            .ToListAsync();
    }

    public async Task<int> GetAvailableCountAsync(int bookId)
    {
        return await _context.BookCopies
            .CountAsync(bc => bc.BookId == bookId && bc.IsAvailable);
    }

    public async Task<List<BookCopy>> GetAvailableCopiesAsync(int bookId)
    {
        return await _context.BookCopies
            .Where(bc => bc.BookId == bookId && bc.IsAvailable)
            .ToListAsync();
    }

    // UPDATE
    public async Task<BookCopy?> UpdateAsync(int id, bool? isAvailable = null, string? bookCode = null)
    {
        var existing = await _context.BookCopies.FindAsync(id);
        if (existing == null) return null;

        if (bookCode != null)
            existing.BookCode = bookCode;
        if (isAvailable.HasValue)
            existing.IsAvailable = isAvailable.Value;

        await _context.SaveChangesAsync();
        return existing;
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var copy = await _context.BookCopies
            .Include(bc => bc.BorrowBooks)
            .FirstOrDefaultAsync(bc => bc.Id == id);
        if (copy == null) return false;

        // Tidak boleh menghapus copy yang sedang dipinjam (ReturnedDate == null)
        if (copy.BorrowBooks != null && copy.BorrowBooks.Any(bb => bb.ReturnedDate == null))
            throw new InvalidOperationException("Cannot delete book copy that is currently borrowed.");

        _context.BookCopies.Remove(copy);
        await _context.SaveChangesAsync();
        return true;
    }


}