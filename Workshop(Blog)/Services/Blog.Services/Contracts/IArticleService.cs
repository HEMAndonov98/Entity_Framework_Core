namespace AspNetCoreTemplate.Services.Contracts;

using System.Collections.Generic;
using System.Threading.Tasks;

using Blog.Web.ViewModels.Article;

/// <summary>
/// Interface for Blog business logic.
/// </summary>
public interface IArticleService
{
    /// <summary>
    /// Blueprint for adding an Article.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddArticle(ArticleAddViewModel model);

    /// <summary>
    /// Blueprint for retrieving all Articles.
    /// </summary>
    /// <returns></returns>
    IEnumerable<ArticleViewModel> GetAll();

    /// <summary>
    /// Blueprint for retrieving a single Article.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ArticleViewModel> GetArticleAsync(int id);

    /// <summary>
    /// Blueprint for editing an Article.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task EditArticleAsync(ArticleEditViewModel model);

    /// <summary>
    /// Blueprint for deleting an Article.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task DeleteArticleAsync(ArticleViewModel model);
}
