using ComicReader.Data;
using ComicReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicReader.Services
{
    public class SubscribeHistoryService
    {
        private readonly AppDbContext _context;
        public SubscribeHistoryService(AppDbContext context) => _context = context;

        public async Task<SubscribeHistory> CreateSubscriptionAsync(int customerId, int durationDays = 30, string paymentMethod = "QRIS")
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) throw new ArgumentException("Customer not found");

            var start = DateTime.UtcNow;
            var end = start.AddDays(durationDays);
            var history = new SubscribeHistory
            {
                CustomerId = customerId,
                StartDate = start,
                EndDate = end,
                TransactionDate = DateTime.UtcNow,
                PaymentMethod = paymentMethod
            };
            _context.SubscribeHistories.Add(history);

            // Extend if already subscribed
            if (customer.SubscribeEndDate.HasValue && customer.SubscribeEndDate.Value > start)
                customer.SubscribeEndDate = customer.SubscribeEndDate.Value.AddDays(durationDays);
            else
                customer.SubscribeEndDate = end;
            customer.IsSubscribe = true;

            await _context.SaveChangesAsync();
            return history;
        }
        public async Task<List<SubscribeHistory>> GetHistoriesByCustomerIdAsync(int customerId) =>
            await _context.SubscribeHistories.Where(sh => sh.CustomerId == customerId).OrderByDescending(sh => sh.StartDate).ToListAsync();

        public async Task UpdateExpiredSubscriptionsAsync()
        {
            var now = DateTime.UtcNow;
            var expired = await _context.Customers.Where(c => c.SubscribeEndDate < now && c.IsSubscribe).ToListAsync();
            foreach (var c in expired) c.IsSubscribe = false;
            await _context.SaveChangesAsync();
        }
    }
}