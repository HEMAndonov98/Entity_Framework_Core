using System.Threading.Tasks;

using AspNetCoreTemplate.Services.Contracts;
using Blog.Data.Common.Repositories;
using Blog.Data.Models;
using Blog.Web.ViewModels.Article;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTemplate.Services;
/// <summary>
/// Implementation of Article business logic
/// </summary>
public class ArticleService : IArticleService
{

    private IRepository<Article> repository;
    private ILogger<Article> logger;
    
    public ArticleService(IRepository<Article> repository, ILogger<Article> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }
    
    /// <summary>
    /// Service for adding a new Article to the database
    /// </summary>
    /// <param name="model"></param>
    public async Task AddArticle(ArticleAddViewModel model)
    {
        Article newArticle = new Article()
        {
            Title = 
        }
    }
}