namespace AspNetCoreTemplate.Services.Contracts;

using System.Threading.Tasks;

using Blog.Web.ViewModels.Article;
/// <summary>
/// Interface for Blog business logic
/// </summary>
public interface IArticleService
{
    /// <summary>
    /// Blueprint for adding an Article
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddArticle(ArticleAddViewModel model);
}