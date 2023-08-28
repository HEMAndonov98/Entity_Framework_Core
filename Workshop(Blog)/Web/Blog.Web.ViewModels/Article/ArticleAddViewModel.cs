using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AspNetCoreTemplate.Web.ViewModels.Category;
using Blog.Data.Common.Constraints;
using Blog.Web.Common.ArticleErrors;

namespace Blog.Web.ViewModels.Article;

public class ArticleAddViewModel
{
    [Display(Name = "Title")]
    [Required(ErrorMessage = ArticleAddErrorMessages.RequiredField)]
    [StringLength(ArticleConstraints.TitleMaxLength,
        MinimumLength = ArticleConstraints.TitleMinLength,
        ErrorMessage = ArticleAddErrorMessages.IncorrectFieldLength)]
    public string Title { get; set; } = null!;

    [Display(Name = "Content")]
    [Required(ErrorMessage = ArticleAddErrorMessages.RequiredField)]
    [StringLength(ArticleConstraints.ContentMaxLength,
        MinimumLength = ArticleConstraints.ContentMinLength,
        ErrorMessage = ArticleAddErrorMessages.IncorrectFieldLength)]
    public string Content { get; set; }

    public int CategoryId { get; set; }

    public ICollection<CategoryViewModel> Categories { get; set; }
}