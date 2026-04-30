using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services;
public class VisitorService
{
    private readonly LibraryDbContext _context;

    public VisitorService(LibraryDbContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<Visitor> CreateAsync(Visitor visitor)
    {
        if (!await IsEmailUniqueAsync(visitor.Email))
            throw new InvalidOperationException("Email already exists.");

        _context.Visitors.Add(visitor);
        await _context.SaveChangesAsync();
        return visitor;
    }

    // READ
    public async Task<List<Visitor>> GetAllAsync()
    {
        return await _context.Visitors.ToListAsync();
    }

    public async Task<Visitor?> GetByIdAsync(int id)
    {
        return await _context.Visitors.FindAsync(id);
    }

    public async Task<Visitor?> GetByEmailAsync(string email)
    {
        return await _context.Visitors.FirstOrDefaultAsync(v => v.Email == email);
    }

    // UPDATE
    public async Task<Visitor?> UpdateAsync(int id, Visitor updatedVisitor)
    {
        var existing = await _context.Visitors.FindAsync(id);
        if (existing == null) return null;

        if (!await IsEmailUniqueAsync(updatedVisitor.Email, id))
            throw new InvalidOperationException("Email already exists.");

        existing.Name = updatedVisitor.Name;
        existing.Email = updatedVisitor.Email;
        existing.PhoneNumber = updatedVisitor.PhoneNumber;
        existing.BirthDate = updatedVisitor.BirthDate;
        existing.IsLibraryMember = updatedVisitor.IsLibraryMember;

        await _context.SaveChangesAsync();
        return existing;
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var visitor = await _context.Visitors
            .Include(v => v.BorrowBooks)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (visitor == null) return false;

        // Cek apakah masih ada peminjaman aktif
        if (visitor.BorrowBooks != null && visitor.BorrowBooks.Any(bb => bb.ReturnedDate == null))
            throw new InvalidOperationException("Cannot delete visitor with active borrows.");

        _context.Visitors.Remove(visitor);
        await _context.SaveChangesAsync();
        return true;
    }

    // Helper
    private async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
    {
        var query = _context.Visitors.Where(v => v.Email == email);
        if (excludeId.HasValue)
            query = query.Where(v => v.Id != excludeId.Value);
        return !await query.AnyAsync();
    }

}