using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ComicReader.Models;

namespace ComicReader.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Customer>>();

        await context.Database.EnsureCreatedAsync();

        // Helper untuk menambah customer jika belum ada
        async Task<Customer> AddCustomerIfNotExists(
            string email,
            string name,
            string phone,
            string role,
            bool isSubscribe,
            DateTime? subscribeEndDate,
            string plainPassword)
        {
            var existing = await context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (existing != null)
            {
                Console.WriteLine($"Customer already exists: {email}");
                return existing;
            }

            var customer = new Customer
            {
                Name = name,
                Email = email,
                PhoneNumber = phone,
                Role = role,
                IsSubscribe = isSubscribe,
                SubscribeEndDate = subscribeEndDate
            };
            customer.PasswordHash = passwordHasher.HashPassword(customer, plainPassword);

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync(); // Simpan agar mendapatkan Id
            Console.WriteLine($"Added new customer: {email}");
            return customer;
        }

        // 1. Admin
        var admin = await AddCustomerIfNotExists(
            email: "admin@comicreader.com",
            name: "Admin User",
            phone: "081234567890",
            role: "Admin",
            isSubscribe: false,
            subscribeEndDate: null,
            plainPassword: "Admin@123"
        );

        // 2. Free User (tidak subscribe)
        var freeUser = await AddCustomerIfNotExists(
            email: "free@comicreader.com",
            name: "Free User",
            phone: "081298765432",
            role: "User",
            isSubscribe: false,
            subscribeEndDate: null,
            plainPassword: "Free@123"
        );

        // 3. Subscribed User (subscribe mulai hari ini, 30 hari)
        var subUser = await AddCustomerIfNotExists(
            email: "subscribed@comicreader.com",
            name: "Subscribed User",
            phone: "081255555555",
            role: "User",
            isSubscribe: true,
            subscribeEndDate: DateTime.UtcNow.AddDays(30),
            plainPassword: "Sub@123"
        );

        // Tambahkan SubscribeHistory untuk subscribed user (jika belum ada)
        if (subUser != null)
        {
            var historyExists = await context.SubscribeHistories.AnyAsync(h => h.CustomerId == subUser.Id);
            if (!historyExists)
            {
                var history = new SubscribeHistory
                {
                    CustomerId = subUser.Id,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                    TransactionDate = DateTime.UtcNow,
                    PaymentMethod = "QRIS"
                };
                await context.SubscribeHistories.AddAsync(history);
                await context.SaveChangesAsync();
                Console.WriteLine($"Added SubscribeHistory for subscribed user (start today).");
            }
            else
            {
                Console.WriteLine($"SubscribeHistory already exists for subscribed user.");
            }
        }
        Console.WriteLine("Seeding completed.");
    }
}