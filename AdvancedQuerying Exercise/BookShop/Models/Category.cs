using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Models;

public class Category
{

    public Category(ICollection<BookCategory> categoryBooks)
    {
        this.CategoryBooks = categoryBooks;
    }

    public Category()
    {
        this.CategoryBooks = new List<BookCategory>();
    }
    
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(EntityValidations.CategoryNameLength)]
    public string Name { get; set; }
    
    [InverseProperty(nameof(Category))]
    public virtual ICollection<BookCategory> CategoryBooks { get; set; }
}