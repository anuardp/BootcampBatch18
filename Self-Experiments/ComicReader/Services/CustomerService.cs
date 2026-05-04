using ComicReader.Data;
using ComicReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicReader.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;
        public CustomerService(AppDbContext context) => _context = context;

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
                throw new InvalidOperationException("Email already exists");
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllCustomersAsync() =>
            await _context.Customers.Include(c => c.SubscribeHistories).OrderBy(c => c.Name).ToListAsync();

        public async Task<Customer?> GetCustomerByIdAsync(int id) =>
            await _context.Customers.Include(c => c.SubscribeHistories).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Customer?> GetCustomerByEmailAsync(string email) =>
            await _context.Customers.Include(c => c.SubscribeHistories).FirstOrDefaultAsync(c => c.Email == email);

        public async Task<bool> UpdateCustomerAsync(int id, Customer updated)
        {
            var cust = await _context.Customers.FindAsync(id);
            if (cust == null) return false;
            cust.Name = updated.Name;
            cust.Email = updated.Email;
            cust.PhoneNumber = updated.PhoneNumber;
            cust.Role = updated.Role;
            cust.IsSubscribe = updated.IsSubscribe;
            cust.SubscribeEndDate = updated.SubscribeEndDate;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var cust = await _context.Customers.FindAsync(id);
            if (cust == null) return false;
            _context.Customers.Remove(cust);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool IsSubscribed(Customer customer) =>
            customer.SubscribeEndDate.HasValue && customer.SubscribeEndDate.Value > DateTime.UtcNow;
    }
}