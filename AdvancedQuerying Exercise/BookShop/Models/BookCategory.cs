using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class BookCategory
{
    [Key]
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }

    public virtual Book Book { get; set; }

    [Key]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }
}