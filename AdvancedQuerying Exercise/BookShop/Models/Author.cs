using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Models;

public class Author
{
    public Author(ICollection<Book> books)
    {
        this.Books = books;
    }

    public Author()
    {
        this.Books = new List<Book>();
    }
    
    [Key]
    public int AuthorId { get; set; }

    [MaxLength(EntityValidations.AuthorFirstNameLength)]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(EntityValidations.AuthorLastNameLength)]
    public string LastName { get; set; }
    
    [InverseProperty(nameof(Author))]
    public virtual ICollection<Book> Books { get; set; }
}