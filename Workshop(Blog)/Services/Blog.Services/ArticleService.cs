namespace AspNetCoreTemplate.Services;

using System;
using System.Threading.Tasks;

using Contracts;
using Blog.Data.Common.Repositories;
using Blog.Data.Models;
using Blog.Web.ViewModels.Article;

/// <summary>
/// Implementation of Article business logic
/// </summary>
public class ArticleService : IArticleService
{

    private IRepository<Article> repository;
    
    public ArticleService(IRepository<Article> repository)
    {
        this.repository = repository;
    }
    
    /// <summary>
    /// Service for adding a new Article to the database
    /// </summary>
    /// <param name="model"></param>
    public async Task AddArticle(ArticleAddViewModel model)
    {
        Article newArticle = new Article()
        {
            Title = model.Title,
            Content = model.Content,
            CreatedOn = DateTime.Now,
            ModifiedOn = DateTime.Now,
            CategoryId = model.CategoryId
        };

        await this.repository.AddAsync(newArticle);
        await this.repository.SaveChangesAsync();
    }
}