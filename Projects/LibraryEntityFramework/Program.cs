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
            Console.WriteLine("=== Library Management System - EF Core===\n");

            using var context = new LibraryDbContext();
            
            await ApplyMigrationsAsync(context);
            
            await SeedDatabaseAsync(context);

            // Initialize services
            var visitorService = new VisitorService(context);
            var bookService = new BookService(context);
            var bookCopyService = new BookCopyService(context);
            var fineService = new FineService(context);
            var borrowBookService = new BorrowBookService(context, fineService);
            try
            {   
                
                await DemonstrateCrudOperationsAsync(bookService, visitorService, bookCopyService);
                await DemonstrateBorrowingProcessAsync(borrowBookService, visitorService, bookCopyService);
                await DemonstrateFineManagementAsync(fineService, borrowBookService);
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

        static async Task DemonstrateCrudOperationsAsync(BookService bookService, VisitorService visitorService, BookCopyService bookCopyService)
        {
            Console.WriteLine("=== CRUD Operations Demo ===");
            Console.WriteLine("\n1. Adding a new book...");
            var newBook = new Book
            {
                BookISBN = "978-1-098-12345-6",
                BookTitle = "Clean Code",
                BookPublisher = "Prentice Hall",
                BookAuthors = "Robert C. Martin",
                Genre = "Software Engineering",
                YearReleased = 2008
            };
            var createdBook = await bookService.CreateAsync(newBook);
            Console.WriteLine($"✓ Added: {createdBook.BookTitle} (ID: {createdBook.Id})");

 
            Console.WriteLine("\n2. Listing all books:");
            var allBooks = await bookService.GetAllAsync();
            foreach (var book in allBooks)
            {
                Console.WriteLine($"  - {book.BookTitle} by {book.BookAuthors} ({book.YearReleased})");
            }
            Console.WriteLine("\n3. Updating book publisher.");
            createdBook.BookPublisher = "Pearson Education";
            var updatedBook = await bookService.UpdateAsync(createdBook.Id, createdBook);
            if (updatedBook != null)
                Console.WriteLine($"✓ Updated publisher to: {updatedBook.BookPublisher}");


            Console.WriteLine("\n4. Adding a new visitor...");
            var newVisitor = new Visitor
            {
                Name = "Alice Brown",
                Email = "alice@example.com",
                PhoneNumber = "081377788899",
                BirthDate = new DateTime(2000, 7, 25),
                IsLibraryMember = true
            };
            var createdVisitor = await visitorService.CreateAsync(newVisitor);
            Console.WriteLine($"✓ Added visitor: {createdVisitor.Name} (ID: {createdVisitor.Id})");

            Console.WriteLine("\n5. Deleting a visitor (if has no active borrows)...");
            var visitorToDelete = await visitorService.GetByIdAsync(createdVisitor.Id);
            if (visitorToDelete != null)
            {
      
                var deleted = await visitorService.DeleteAsync(visitorToDelete.Id);
                if (deleted)
                    Console.WriteLine($"✓ Deleted visitor: {visitorToDelete.Name}");
                else
                    Console.WriteLine($"✗ Could not delete {visitorToDelete.Name} (may have active borrows)");
            }
            Console.WriteLine("\n--- CRUD Operations Complete ---\n");
        }
        static async Task DemonstrateBorrowingProcessAsync(BorrowBookService borrowService, VisitorService visitorService, BookCopyService copyService)
        {
            Console.WriteLine("=== Borrowing & Returning Process ===");

            var visitors = await visitorService.GetAllAsync();
            var visitor = visitors.FirstOrDefault(v => v.IsLibraryMember);
            if (visitor == null)
            {
                Console.WriteLine("No eligible visitor found.");
                return;
            }

            var availableCopies = await copyService.GetAvailableCopiesAsync(1); // assume book ID 1 has copies
            var copy = availableCopies.FirstOrDefault();
            if (copy == null)
            {
                Console.WriteLine("No available book copies.");
                return;
            }

            Console.WriteLine($"\n1. Borrowing book copy '{copy.BookCode}' for visitor '{visitor.Name}'...");
            var borrow = await borrowService.BorrowAsync(copy.Id, visitor.Id, loanDurationDays: 7);
            Console.WriteLine($"✓ Borrowed successfully! Due date: {borrow.BorrowBookDue:yyyy-MM-dd}");

            Console.WriteLine("\n2. Active borrows:");
            var activeBorrows = await borrowService.GetActiveBorrowsAsync();
            foreach (var b in activeBorrows)
            {
                Console.WriteLine($"   - {b.Visitor?.Name} borrowed '{b.BookCopy?.Book?.BookTitle}' until {b.BorrowBookDue:yyyy-MM-dd}");
            }

            Console.WriteLine("\n3. Returning the book...");
            var returned = await borrowService.ReturnBookAsync(borrow.Id);
            if (returned != null)
            {
                Console.WriteLine($"✓ Returned on {returned.ReturnedDate:yyyy-MM-dd}");
                if (returned.Fine != null && returned.Fine.TotalFine > 0)
                    Console.WriteLine($"   Note: Late fine of {returned.Fine.TotalFine:C} applied.");
                else
                    Console.WriteLine("   No fines.");
            }

            
            var copiesNow = await copyService.GetByIdAsync(copy.Id);
            Console.WriteLine($"\n4. Book copy '{copiesNow?.BookCode}' availability after return: {(copiesNow?.IsAvailable == true ? "Available" : "Not available")}");
            Console.WriteLine("\n--- Borrowing & Returning Complete ---\n");
        }
        static async Task DemonstrateFineManagementAsync(FineService fineService, BorrowBookService borrowService)
        {
            Console.WriteLine("=== Fine Management Demo ===");
            var unpaidFines = await fineService.GetUnpaidFinesAsync();
            Console.WriteLine("\n1. Unpaid fines:");
            if (!unpaidFines.Any())
            {
                Console.WriteLine("   No unpaid fines.");
            }
            else
            {
                foreach (var fine in unpaidFines)
                {
                    Console.WriteLine($"   - Fine ID: {fine.Id}, Amount: {fine.TotalFine:C}, Visitor: {fine.BorrowBook?.Visitor?.Name ?? "N/A"}");
                }
            }

            var firstFine = unpaidFines.FirstOrDefault();
            if (firstFine != null)
            {
                Console.WriteLine($"\n2. Paying fine of {firstFine.TotalFine:C}...");
                var paid = await fineService.PayFineAsync(firstFine.Id);
                if (paid)
                {
                    Console.WriteLine($"✓ Fine paid on {DateTime.Now:yyyy-MM-dd}");
                    var updatedFine = await fineService.GetByIdAsync(firstFine.Id);
                    Console.WriteLine($"   Status: {(updatedFine?.HasPayFine == true ? "Paid" : "Unpaid")}");
                }
            }


            var visitors = await new VisitorService(new LibraryDbContext()).GetAllAsync();
            var visitor = visitors.FirstOrDefault();
            if (visitor != null)
            {
                var outstanding = await fineService.GetOutstandingFinesByVisitorAsync(visitor.Id);
                Console.WriteLine($"\n3. Outstanding fines for {visitor.Name}: {outstanding:C}");
            }

            Console.WriteLine("\n--- Fine Management Complete ---\n");
        }
    }
}