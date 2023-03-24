using Data;
using DbInitialiszer.Generators;
using Models;

namespace DbInitialiszer;

public static class DbIntitializer
{
    public static  void ResetDatabase(BookShopContext context)
    {
          context.Database.EnsureDeleted();
          context.Database.EnsureCreated();

         Seed(context);
    }

    private static void Seed(BookShopContext context)
    {
        Book[] books = BookGenerator.CreateBooks();
        
        context.Books.AddRange(books);
        context.SaveChanges();
    }
}