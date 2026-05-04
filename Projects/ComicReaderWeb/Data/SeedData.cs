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

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Customers only if table is empty
        if (!context.Customers.Any())
        {
            // 1. Admin User
            var admin = new Customer
            {
                Name = "Admin User",
                Email = "admin@comicreader.com",
                PhoneNumber = "081234567890",
                Role = "Admin",
                IsSubscribe = false,
                SubscribeEndDate = null
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin@123");

            // 2. Free User (not subscribed)
            var freeUser = new Customer
            {
                Name = "Free User",
                Email = "free@comicreader.com",
                PhoneNumber = "081298765432",
                Role = "User",
                IsSubscribe = false,
                SubscribeEndDate = null
            };
            freeUser.PasswordHash = passwordHasher.HashPassword(freeUser, "Free@123");

            // 3. Subscribed User (subscribed 1 week ago)
            var subUser = new Customer
            {
                Name = "Subscribed User",
                Email = "subscribed@comicreader.com",
                PhoneNumber = "081255555555",
                Role = "User",
                IsSubscribe = true,
                SubscribeEndDate = DateTime.UtcNow.AddDays(23) // 30 - 7 = 23 days remaining
            };
            subUser.PasswordHash = passwordHasher.HashPassword(subUser, "Sub@123");

            await context.Customers.AddRangeAsync(admin, freeUser, subUser);
            await context.SaveChangesAsync();

            // Seed SubscribeHistory for the subscribed user
            var history = new SubscribeHistory
            {
                CustomerId = subUser.Id,
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow.AddDays(23),
                TransactionDate = DateTime.UtcNow.AddDays(-7),
                PaymentMethod = "QRIS"
            };
            await context.SubscribeHistories.AddAsync(history);
            await context.SaveChangesAsync();
        }
    }
}