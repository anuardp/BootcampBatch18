using Microsoft.EntityFrameworkCore;
using ComicReader.Data;
using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customer?> GetByIdWithHistoriesAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.SubscribeHistories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await GetByIdAsync(id);
        if (customer == null) return false;
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Customers.AnyAsync(c => c.Email == email);
    }

    public async Task<List<Customer>> GetSubscribedCustomersAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Customers
            .Where(c => c.IsSubscribe && c.SubscribeEndDate > now)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}