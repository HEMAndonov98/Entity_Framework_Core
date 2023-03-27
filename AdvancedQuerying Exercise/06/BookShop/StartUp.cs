using System.Globalization;
using System.Text;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            Console.WriteLine(GetBooksReleasedBefore(db, Console.ReadLine()));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

                var bookTitles = context.Books
                    .AsNoTracking()
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .Select(b => b.Title)
                    .OrderBy(b => b)
                    .ToArray();

                return string.Join(Environment.NewLine, bookTitles);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            int maxCopies = 5000;
            string[] bookTitles = context.Books
                .AsNoTracking()
                .Where(b => b.EditionType == EditionType.Gold &&
                                 b.Copies < maxCopies)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles).Trim();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            int minBookPrice = 40;
            var booksAndPrices = context.Books
                .AsNoTracking()
                .Where(b => b.Price > minBookPrice)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                    {
                        b.Title,
                        b.Price
                    })
                .ToArray();


            var sb = new StringBuilder();

            foreach (var book in booksAndPrices)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }
            
            return sb.ToString()
                .Trim();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            string[] bookTitles = context.Books
                .AsNoTracking()
                .Where(b => b.ReleaseDate!.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            
            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categoryTokens = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(ct => ct.ToLower())
                .ToArray();

            string[] bookTitles = context.BooksCategories
                .AsNoTracking()
                .Include(bc => bc.Book)
                .Include(bc => bc.Category)
                .Where(bc => categoryTokens.Contains(bc.Category.Name.ToLower()))
                .Select(bc => bc.Book.Title)
                .OrderBy(b => b)
                .ToArray();

            //Another possible solution using lazy loading would have us including only the categories for the first filtering
            //and from then on dynamically load all the book titles from the finished query meaning there would be one filtering query
            //and from that query we would load all the book titles the query will be something like SELECT Title FROM Books WHERE BookId = Result

            return string.Join(Environment.NewLine, bookTitles).Trim();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            try
            {
                var books = context.Books
                    .AsNoTracking()
                    .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                    .OrderByDescending(b => b.ReleaseDate)
                    .Select(b => new
                    {
                        b.Title,
                        b.EditionType,
                        b.Price
                    })
                    .ToList();

                StringBuilder sb = new StringBuilder();

                foreach (var book in books)
                {
                    sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
                }

                return sb.ToString()
                    .Trim();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}


