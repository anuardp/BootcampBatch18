using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Data;

public class LibraryDbContext : DbContext
{
    public DbSet<Visitor> Visitors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BorrowBook> BorrowBooks { get; set; }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
    public LibraryDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=CompanyDatabase.db");
            optionsBuilder.EnableSensitiveDataLogging();
           }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}
        