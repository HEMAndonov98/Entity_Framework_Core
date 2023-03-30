using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace ProductShop.Models
{
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
            CategoriesProducts = new List<CategoryProduct>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        [InverseProperty(nameof(Category))]
        public virtual ICollection<CategoryProduct> CategoriesProducts { get; set; }
    }
}
