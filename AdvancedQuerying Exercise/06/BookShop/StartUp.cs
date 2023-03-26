using System;
using System.Linq;
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
            Console.WriteLine(GetBooksByPrice(db));
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
    }
}


