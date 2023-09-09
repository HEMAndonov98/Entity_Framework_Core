using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Data.Common.Constraints;
using Blog.Data.Common.Models;

namespace Blog.Data.Models;
/// <summary>
/// This model represents a single article
/// </summary>
public class Article : BaseModel<int>
{
    /// <summary>
    /// Article Title
    /// </summary>
    [MaxLength(ArticleConstraints.TitleMaxLength)]
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Content written in an article
    /// </summary>
    [MaxLength(ArticleConstraints.ContentMaxLength)]
    [Required]
    public string Content { get; set; } = null!;

    /// <summary>
    /// Article Creator
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Category Id
    /// </summary>
   [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public Category Category { get; set; }
}