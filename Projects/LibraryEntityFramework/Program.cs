using Entity_Framework.Data;
using Entity_Framework.Models;
using Entity_Framework.Services;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Library Management System - EF Core Demo ===\n");

            using var context = new LibraryDbContext();
            
            await ApplyMigrationsAsync(context);
            
            await SeedDatabaseAsync(context);

            // Initialize services
            var visitorService = new VisitorService(context);
            var bookService = new BookService(context);
            var bookCopyService = new BookCopyService(context);
            var fineService = new FineService(context);
            var borrowBookService = new BorrowBookService(context);

            try
            {
                
                Console.WriteLine("\n=== Demo completed successfully! ===");
                Console.WriteLine("Check the LibraryDatabase.db file created in your project folder.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError occurred: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner error: {ex.InnerException.Message}");
            }
        }

        static async Task ApplyMigrationsAsync(LibraryDbContext context)
        {
            Console.WriteLine("Applying database migrations...");
            await context.Database.EnsureCreatedAsync(); // Membuat database jika belum ada
            Console.WriteLine("Database is ready.\n");
        }
        static async Task SeedDatabaseAsync(LibraryDbContext context)
        {
            if (await context.Books.AnyAsync())
            {
                Console.WriteLine("Database already contains data, skipping seeding.\n");
                return;
            }

            Console.WriteLine("Seeding database with initial data...");

            var books = new[]
            {
                new Book { BookISBN = "978-0-13-110362-7", BookTitle = "The C Programming Language", BookPublisher = "Prentice Hall", BookAuthors = "Brian W. Kernighan, Dennis M. Ritchie", Genre = "Programming", YearReleased = 1988 },
                new Book { BookISBN = "978-0-201-63361-0", BookTitle = "Design Patterns", BookPublisher = "Addison-Wesley", BookAuthors = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides", Genre = "Software Engineering", YearReleased = 1994 },
                new Book { BookISBN = "978-1-491-91739-0", BookTitle = "C# 10 in a Nutshell", BookPublisher = "O'Reilly Media", BookAuthors = "Joseph Albahari", Genre = "Programming", YearReleased = 2022 },
                new Book { BookISBN = "978-1-59327-599-0", BookTitle = "Automate the Boring Stuff with Python", BookPublisher = "No Starch Press", BookAuthors = "Al Sweigart", Genre = "Programming", YearReleased = 2015 }
            };
            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();

            var visitors = new[]
            {
                new Visitor { Name = "John Doe", Email = "john.doe@example.com", PhoneNumber = "081234567890", BirthDate = new DateTime(1990, 5, 15), IsLibraryMember = true },
                new Visitor { Name = "Jane Smith", Email = "jane.smith@example.com", PhoneNumber = "081298765432", BirthDate = new DateTime(1985, 10, 20), IsLibraryMember = true },
                new Visitor { Name = "Bob Johnson", Email = "bob.johnson@example.com", PhoneNumber = "081355512345", BirthDate = new DateTime(1995, 3, 8), IsLibraryMember = false }
            };
            await context.Visitors.AddRangeAsync(visitors);
            await context.SaveChangesAsync();


            var firstBook = await context.Books.FirstAsync();
            var secondBook = await context.Books.Skip(1).FirstAsync();
            var thirdBook = await context.Books.Skip(2).FirstAsync();
            var fourthBook = await context.Books.Skip(3).FirstAsync();

            var bookCopies = new[]
            {
                new BookCopy { BookId = firstBook.Id, BookCode = "C001-01", IsAvailable = true },
                new BookCopy { BookId = firstBook.Id, BookCode = "C001-02", IsAvailable = true },
                new BookCopy { BookId = secondBook.Id, BookCode = "DP001-01", IsAvailable = true },
                new BookCopy { BookId = thirdBook.Id, BookCode = "CS001-01", IsAvailable = true },
                new BookCopy { BookId = fourthBook.Id, BookCode = "PY001-01", IsAvailable = true }
            };
            await context.BookCopies.AddRangeAsync(bookCopies);
            await context.SaveChangesAsync();

            Console.WriteLine("Database seeding completed!\n");
        }
    }
}