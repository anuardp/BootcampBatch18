using ComicReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicReader.Data;
public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<SubscribeHistory> SubscribeHistories { get; set; }
    public DbSet<Comic> Comics { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Page> Pages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public AppDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=ComicReader.db");
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique email
        modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();

        // Customer -> SubscribeHistory
        modelBuilder.Entity<SubscribeHistory>()
            .HasOne(sh => sh.Customer)
            .WithMany(c => c.SubscribeHistories)
            .HasForeignKey(sh => sh.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Comic -> Chapter
        modelBuilder.Entity<Chapter>()
            .HasOne(ch => ch.Comic)
            .WithMany(c => c.Chapters)
            .HasForeignKey(ch => ch.ComicId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Chapter>()
            .HasIndex(ch => new { ch.ComicId, ch.ChapterNumber }).IsUnique();

        // Chapter -> Page
        modelBuilder.Entity<Page>()
            .HasOne(p => p.Chapter)
            .WithMany(ch => ch.Pages)
            .HasForeignKey(p => p.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Page>()
            .HasIndex(p => new { p.ChapterId, p.PageNumber }).IsUnique();

        // Index for foreign keys
        modelBuilder.Entity<SubscribeHistory>().HasIndex(sh => sh.CustomerId);
        modelBuilder.Entity<Chapter>().HasIndex(ch => ch.ComicId);
        modelBuilder.Entity<Page>().HasIndex(p => p.ChapterId);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comic>().HasData(
            new Comic { Id = 1, Title = "Sample Comic", Author = "Author Name", Genre = "Action", IsPremium = false, TotalChapter = 0, DateAdded = DateTime.UtcNow }
        );
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Admin User", Email = "admin@example.com", Role = "Admin", IsSubscribe = false, SubscribeEndDate = null }
        );
    }
}