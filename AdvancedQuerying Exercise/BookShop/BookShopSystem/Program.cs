using System.ComponentModel;
using Data;
using DbInitialiszer;
using Microsoft.EntityFrameworkCore;
using Models.Enums;

namespace BookShop
{
    public class StartUp
    {
        public static  void Main(string[] args)
        {
             using var context = new BookShopContext();
             //DbIntitializer.ResetDatabase(context);

             try
             {
                 string input = "Normal";
                 EditionType editionType = default;
                 if (!EditionType.TryParse(input, true, out editionType))
                 {
                     throw new InvalidEnumArgumentException();
                 }
             }
             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
                 throw;
             }
             


             var specific = context.Books
                 .AsNoTracking()
                 .ToArray();
        }
    }
}

