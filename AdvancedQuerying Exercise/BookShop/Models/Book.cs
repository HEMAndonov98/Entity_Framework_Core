using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Models.Enums;

namespace Models;

public class Book
{
    public Book(ICollection<BookCategory> bookCategories)
    {
        this.BookCategories = bookCategories;
    }

    public Book()
    {
        this.BookCategories = new List<BookCategory>();
    }
    
    [Key]
    public int BookId { get; set; }

    [Required]
    [MaxLength(EntityValidations.BookTitleLength)]
    public string Title { get; set; }

    [Required]
    [MaxLength(EntityValidations.BookDescriptionLength)]
    public string Description { get; set; }
    
    public DateTime? ReleaseDate { get; set; }

    public int Copies { get; set; }

    public decimal Price { get; set; }

    public EditionType EditionType { get; set; }

    public AgeRestriction AgeRestriction { get; set; }

    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }

    public virtual Author Author { get; set; }
    
    [InverseProperty(nameof(Book))]
    public virtual ICollection<BookCategory> BookCategories { get; set; }
}