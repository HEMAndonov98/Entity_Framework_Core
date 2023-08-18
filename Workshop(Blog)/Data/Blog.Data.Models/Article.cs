using System.ComponentModel.DataAnnotations;
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
    [MinLength(ArticleConstraints.TitleMinLength)]
    public string Type { get; set; } = null!;

    /// <summary>
    /// Content written in an article
    /// </summary>
    [MinLength(ArticleConstraints.ContentMinLength)]
    public string Content { get; set; } = null!;

    /// <summary>
    /// Article Creator
    /// </summary>
    public string Author { get; set; }
}