using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Name { get; set; } = null!;

        [InverseProperty(nameof(Category))]
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
