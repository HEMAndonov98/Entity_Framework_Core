using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductShop.Models
{
    using System.Collections.Generic;
    using Common;

    public class User
    {
        public User()
        {
            this.ProductsSold = new List<Product>();
            this.ProductsBought = new List<Product>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(EntityValidations.UserFirstNameMaxLength)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(EntityValidations.UserLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        public int? Age { get; set; }

        [InverseProperty(nameof(Product.Buyer))]
        public ICollection<Product> ProductsSold { get; set; }
        [InverseProperty(nameof(Product.Seller))]
        public ICollection<Product> ProductsBought { get; set; }
    }
}