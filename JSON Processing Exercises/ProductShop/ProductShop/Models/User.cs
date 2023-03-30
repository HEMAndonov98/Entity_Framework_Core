using System.ComponentModel.DataAnnotations;
using Common;

namespace ProductShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public User()
        {
            ProductsSold = new List<Product>();
            ProductsBought = new List<Product>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(EntityValidations.UsersFirstNameMaxLength)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(EntityValidations.UsersLastNameMaxLength)]
        public string LastName { get; set; } = null!;
        
        public int? Age { get; set; }

        [InverseProperty("Seller")]
        public virtual ICollection<Product> ProductsSold { get; set; }

        [InverseProperty("Buyer")]
        public virtual ICollection<Product> ProductsBought { get; set; }
    }
}