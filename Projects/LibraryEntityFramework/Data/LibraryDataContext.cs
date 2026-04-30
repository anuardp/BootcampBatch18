using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Data;
public class LibraryDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<BookCopy> BookCopies { get; set; }
    public DbSet<Visitor> Visitors { get; set; }
    public DbSet<BorrowBook> BorrowBooks { get; set; }
    public DbSet<Fine> Fines { get; set; }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
    public LibraryDbContext() { } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=LibraryDatabase.db");
                optionsBuilder.EnableSensitiveDataLogging();
               }
        }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookCopy>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCopies)
            .HasForeignKey(bc => bc.BookID);
        
        modelBuilder.Entity<BorrowBook>()
            .HasOne(bb => bb.BookCopy)
            .WithMany(bc => bc.BorrowBooks)
            .HasForeignKey(bb => bb.BookCopyID);
        
        modelBuilder.Entity<BorrowBook>()
            .HasOne(bb => bb.Visitor)
            .WithMany(v => v.BorrowBooks)
            .HasForeignKey(bb => bb.VisitorID);
        
        modelBuilder.Entity<Fine>()
            .HasOne(f => f.BorrowBook)
            .WithOne(bb => bb.Fine)
            .HasForeignKey<Fine>(f => f.BorrowID);
        
        modelBuilder.Entity<BookCopy>()
            .HasIndex(bc => bc.BookID); 

        modelBuilder.Entity<BorrowBook>()
            .HasIndex(bb => bb.VisitorID);

        modelBuilder.Entity<BorrowBook>()
            .HasIndex(bb => bb.BorrowBookStart); 
    }
}