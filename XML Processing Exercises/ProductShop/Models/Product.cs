using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductShop.Common;

namespace ProductShop.Models
{
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
            this.CategoryProducts = new List<CategoryProduct>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Column(TypeName = EntityValidations.ProductPrice)]
        public decimal Price { get; set; }

        [ForeignKey(nameof(Seller))]
        public int SellerId { get; set; }
        public User Seller { get; set; } = null!;

        [ForeignKey(nameof(Buyer))]
        public int? BuyerId { get; set; }

        public User? Buyer { get; set; }

        [InverseProperty(nameof(Product))]
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}