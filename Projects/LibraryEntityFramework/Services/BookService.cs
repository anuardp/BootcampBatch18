using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services;
public class BookService
{
    private readonly LibraryDbContext _context;

    public BookService(LibraryDbContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<Book> CreateAsync(Book book)
    {
        if (await _context.Books.AnyAsync(b => b.BookISBN == book.BookISBN))
            throw new InvalidOperationException("Book with same ISBN already exists.");

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    // READ
    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.BookCopies)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.BookCopies)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Book>> SearchAsync(string? title, string? author, string? genre)
    {
        var query = _context.Books.AsQueryable();
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.BookTitle.Contains(title));
        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => b.BookAuthors.Contains(author));
        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(b => b.Genre.Contains(genre));
        return await query.ToListAsync();
    }

    // UPDATE
    public async Task<Book?> UpdateAsync(int id, Book updatedBook)
    {
        var existing = await _context.Books.FindAsync(id);
        if (existing == null) return null;

        // Cek ISBN jika diubah
        if (existing.BookISBN != updatedBook.BookISBN &&
            await _context.Books.AnyAsync(b => b.BookISBN == updatedBook.BookISBN))
            throw new InvalidOperationException("Book with same ISBN already exists.");

        existing.BookISBN = updatedBook.BookISBN;
        existing.BookTitle = updatedBook.BookTitle;
        existing.BookPublisher = updatedBook.BookPublisher;
        existing.BookAuthors = updatedBook.BookAuthors;
        existing.Genre = updatedBook.Genre;
        existing.YearReleased = updatedBook.YearReleased;

        await _context.SaveChangesAsync();
        return existing;
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books
            .Include(b => b.BookCopies)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) return false;

        // Cek apakah masih ada copy yang dipinjam atau belum dikembalikan
        if (book.BookCopies != null && book.BookCopies.Any(c => !c.IsAvailable))
            throw new InvalidOperationException("Cannot delete book because some copies are currently borrowed.");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }


}