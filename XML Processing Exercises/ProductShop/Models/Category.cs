using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductShop.Common;

namespace ProductShop.Models
{
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
            this.CategoryProducts = new List<CategoryProduct>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        [InverseProperty(nameof(Category))]
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
