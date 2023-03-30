using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProductShop.Models;
namespace ProductShop.Data
{
    public class ProductShopContext : DbContext
    {
        public ProductShopContext()
        {
        }

        public ProductShopContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CategoryProduct> CategoriesProducts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString)
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryProduct>(entity => { entity.HasKey(x => new { x.CategoryId, x.ProductId }); });

            modelBuilder.Entity<CategoryProduct>(cp => cp
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CategoriesProducts)
                .HasForeignKey(cp => cp.CategoryId));

            modelBuilder.Entity<CategoryProduct>(cp => cp
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CategoriesProducts)
                .HasForeignKey(cp => cp.ProductId));


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(x => x.ProductsBought)
                    .WithOne(x => x.Buyer)
                    .HasForeignKey(x => x.BuyerId);

                entity.HasMany(x => x.ProductsSold)
                    .WithOne(x => x.Seller)
                    .HasForeignKey(x => x.SellerId);
            });

            modelBuilder.Entity<Product>(p => p
                .Property(p => p.Price)
                .HasColumnType(
                    $"DECIMAL({EntityValidations.ProductPriceDecimal}, {EntityValidations.ProductPriceDecimalAfterSeparator})"));

            modelBuilder.Entity<Product>(p => p
                .HasOne(p => p.Seller)
                .WithMany(s => s.ProductsSold)
                .OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Product>(p => p
                .HasOne(p => p.Buyer)
                .WithMany(u => u.ProductsBought)
                .OnDelete(DeleteBehavior.SetNull));
        }
    }
}
