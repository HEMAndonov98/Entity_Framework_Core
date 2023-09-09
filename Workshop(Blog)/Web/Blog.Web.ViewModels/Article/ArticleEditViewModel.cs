using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AspNetCoreTemplate.Web.ViewModels.Category;
using Blog.Data.Common.Constraints;
using Blog.Web.Common.ArticleErrors;

namespace Blog.Web.ViewModels.Article;

public class ArticleEditViewModel
{
    /// <summary>
    /// identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Article Title
    /// </summary>
    [Display(Name = "Title")]
    [Required(ErrorMessage = ArticleAddErrorMessages.RequiredField)]
    [StringLength(ArticleConstraints.TitleMaxLength,
        MinimumLength = ArticleConstraints.TitleMinLength,
        ErrorMessage = ArticleAddErrorMessages.IncorrectFieldLength)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Article Description
    /// </summary>
    [Display(Name = "Content")]
    [Required(ErrorMessage = ArticleAddErrorMessages.RequiredField)]
    [StringLength(ArticleConstraints.ContentMaxLength,
        MinimumLength = ArticleConstraints.ContentMinLength,
        ErrorMessage = ArticleAddErrorMessages.IncorrectFieldLength)]
    public string Content { get; set; } = null!;

    public int CategoryId { get; set; }

    public string Author { get; set; } = null!;

    public ICollection<CategoryViewModel> Categories { get; set; }

    public DateTime CreatedOn { get; set; }
}
