using System;

namespace Blog.Web.ViewModels.Article;
/// <summary>
/// View model for visualizing an article
/// </summary>
public class ArticleViewModel
{
    /// <summary>
    /// identifier
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Article Title
    /// </summary>
    public string Title { get; set; } = null!;
    
    /// <summary>
    /// Article Description
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// Date of Article creation
    /// </summary>
    public string CreatedOn { get; set; }

    /// <summary>
    /// UserName of creator
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Article Category group
    /// </summary>
    public string Category { get; set; }
}