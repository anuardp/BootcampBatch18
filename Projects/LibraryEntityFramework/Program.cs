using Entity_Framework.Data;
using Entity_Framework.Models;
using Entity_Framework.Services;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework;
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Entity Framework Core Demo ===");

        using var context = new LibraryDbContext();
            
        // Apply migrations
        await context.Database.MigrateAsync();
            
        // Seed data
        await SeedDatabaseAsync(context);

        // Initialize services
        var vistorService = new VisitorService(context);
        var bookService = new BookService(context);
        var borrowBookService = new BorrowBookService(context);
        var fineservice = new FineService(context);
        var bookCopyService = new BookCopyService(context);

        // Demo CRUD operations

        
        
    }

    static async Task SeedDatabaseAsync(LibraryDbContext context)
    {
        if (await context.Books.AnyAsync()) return;
        var books = new[]
        {
            new Book 
            { 
                BookISBN = "978-0-13-110362-7", 
                BookTitle = "The C Programming Language", 
                BookPublisher = "Prentice Hall", 
                BookAuthors = "Brian W. Kernighan, Dennis M. Ritchie", 
                Genre = "Programming", 
                YearReleased = 1988 
            },
            new Book 
            { 
                BookISBN = "978-0-201-63361-0", 
                BookTitle = "Design Patterns", 
                BookPublisher = "Addison-Wesley", 
                BookAuthors = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides", 
                Genre = "Software Engineering", 
                YearReleased = 1994 
            },
            new Book 
            { 
                BookISBN = "978-0-596-00535-1", 
                BookTitle = "Head First Design Patterns", 
                BookPublisher = "O'Reilly Media", 
                BookAuthors = "Eric Freeman, Elisabeth Robson", 
                Genre = "Software Engineering", 
                YearReleased = 2004 
            },
            new Book 
            { 
                BookISBN = "978-1-491-91739-0", 
                BookTitle = "C# 10 in a Nutshell", 
                BookPublisher = "O'Reilly Media", 
                BookAuthors = "Joseph Albahari", 
                Genre = "Programming", 
                YearReleased = 2022 
            },
            new Book 
            { 
                BookISBN = "978-1-59327-599-0", 
                BookTitle = "Automate the Boring Stuff with Python", 
                BookPublisher = "No Starch Press", 
                BookAuthors = "Al Sweigart", 
                Genre = "Programming", 
                YearReleased = 2015 
            }
        };
        context.Books.AddRange(books);
        await context.SaveChangesAsync();

        if (await context.Visitors.AnyAsync()) return;

        var visitors = new[]
        {
            new Visitor 
            { 
                Name = "Abdul Dudul", 
                Email = "abdul.dulduldul@gmail.com", 
                PhoneNumber = "081234567890", 
                BirthDate = new DateTime(1990, 5, 15), 
                IsLibraryMember = true 
            },
            new Visitor 
            { 
                Name = "Ahmad Kasim", 
                Email = "akasimkasim@gmail.com", 
                PhoneNumber = "081298765432", 
                BirthDate = new DateTime(1985, 10, 20), 
                IsLibraryMember = true 
            },
            new Visitor 
            { 
                Name = "Apakpak", 
                Email = "apakpak@gmail.com", 
                PhoneNumber = "081355512345", 
                BirthDate = new DateTime(1995, 3, 8), 
                IsLibraryMember = false 
            },
            new Visitor 
            { 
                Name = "Incik Bos", 
                Email = "incik.bos.terbaik@gmail.com", 
                PhoneNumber = "081377788899", 
                BirthDate = new DateTime(2000, 7, 25), 
                IsLibraryMember = true 
            }
        };

        context.Visitors.AddRange(visitors);
        await context.SaveChangesAsync();
        
        if (await context.BookCopies.AnyAsync()) return;

        var booksList = await context.Books.ToListAsync();
        var book1 = booksList.First(b => b.BookISBN == "978-0-13-110362-7");
        var book2 = booksList.First(b => b.BookISBN == "978-0-201-63361-0");
        var book3 = booksList.First(b => b.BookISBN == "978-0-596-00535-1");
        var book4 = booksList.First(b => b.BookISBN == "978-1-491-91739-0");
        var book5 = booksList.First(b => b.BookISBN == "978-1-59327-599-0");

        var bookCopies = new[]
        {
            new BookCopy { BookId = book1.Id, BookCode = "C001-A", IsAvailable = true },
            new BookCopy { BookId = book1.Id, BookCode = "C001-B", IsAvailable = true },
            
            new BookCopy { BookId = book2.Id, BookCode = "DP001-A", IsAvailable = true },
            
            new BookCopy { BookId = book3.Id, BookCode = "HFDP001-A", IsAvailable = true },
            new BookCopy { BookId = book3.Id, BookCode = "HFDP001-B", IsAvailable = false },
            
            new BookCopy { BookId = book4.Id, BookCode = "CS001-A", IsAvailable = true },
            
            new BookCopy { BookId = book5.Id, BookCode = "PY001-A", IsAvailable = true },
            new BookCopy { BookId = book5.Id, BookCode = "PY001-B", IsAvailable = true },
            new BookCopy { BookId = book5.Id, BookCode = "PY001-C", IsAvailable = false }
        };

        context.BookCopies.AddRange(bookCopies);
        await context.SaveChangesAsync();
    }
}