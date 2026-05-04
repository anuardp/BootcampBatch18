using Microsoft.EntityFrameworkCore;
using ComicReader.Data;
using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;

public class SubscribeHistoryRepository : ISubscribeHistoryRepository
{
    private readonly AppDbContext _context;

    public SubscribeHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SubscribeHistory?> GetByIdAsync(int id)
    {
        return await _context.SubscribeHistories.FindAsync(id);
    }

    public async Task<List<SubscribeHistory>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.SubscribeHistories
            .Where(sh => sh.CustomerId == customerId)
            .OrderByDescending(sh => sh.StartDate)
            .ToListAsync();
    }

    public async Task<List<SubscribeHistory>> GetAllAsync()
    {
        return await _context.SubscribeHistories
            .Include(sh => sh.Customer)
            .OrderByDescending(sh => sh.TransactionDate)
            .ToListAsync();
    }

    public async Task<SubscribeHistory> CreateAsync(SubscribeHistory history)
    {
        _context.SubscribeHistories.Add(history);
        await _context.SaveChangesAsync();
        return history;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var history = await GetByIdAsync(id);
        if (history == null) return false;
        _context.SubscribeHistories.Remove(history);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<SubscribeHistory>> GetActiveSubscriptionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.SubscribeHistories
            .Include(sh => sh.Customer)
            .Where(sh => sh.EndDate > now)
            .OrderBy(sh => sh.EndDate)
            .ToListAsync();
    }
}