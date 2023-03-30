using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace ProductShop.Models
{
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
            CategoriesProducts = new List<CategoryProduct>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
        
        [ForeignKey(nameof(Seller))]
        public int SellerId { get; set; }
        public virtual User Seller { get; set; } = null!;

        [ForeignKey(nameof(Buyer))]
        public int? BuyerId { get; set; }
        public virtual User? Buyer { get; set; } = null!;

        [InverseProperty(nameof(Product))]
        public virtual ICollection<CategoryProduct> CategoriesProducts { get; set; }
    }
}