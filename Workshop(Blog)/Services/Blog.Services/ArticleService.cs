namespace AspNetCoreTemplate.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contracts;
using Blog.Data;
using Blog.Data.Common.Repositories;
using Blog.Data.Models;
using Blog.Web.ViewModels.Article;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Implementation of Article business logic.
/// </summary>
public class ArticleService : IArticleService
{
    private readonly IRepository<Article> repository;
    private readonly ApplicationDbContext context;

    /// <summary>
    /// Article service constructor.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public ArticleService(IRepository<Article> repository, ApplicationDbContext context)
    {
        this.repository = repository;
        this.context = context;
    }

    /// <summary>
    /// Service for adding a new Article to the database.
    /// </summary>
    /// <param name="model"></param>
    public async Task AddArticle(ArticleAddViewModel model)
    {
        Article newArticle = new Article
        {
            Title = model.Title,
            Content = model.Content,
            CreatedOn = DateTime.Now,
            ModifiedOn = DateTime.Now,
            CategoryId = model.CategoryId,
            Author = model.Author,
        };

        await this.repository.AddAsync(newArticle);
        await this.repository.SaveChangesAsync();
    }

    /// <summary>
    /// Service for retrieving all articles from database.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ArticleViewModel> GetAll()
    {
        var articles = this.repository.AllAsNoTracking()
            .Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CreatedOn = a.CreatedOn,
                Author = a.Author,
                Category = this.context.Categories.FirstOrDefault(c => c.Id == a.CategoryId).Name,
            })
            .ToArray();

        return articles;
    }

    /// <summary>
    /// Service for retrieving a single Article from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ArticleViewModel> GetArticleAsync(int id)
    {
        ArticleViewModel article = await this.repository
            .AllAsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Author = a.Author,
                CreatedOn = a.CreatedOn,
                Category = this.context
                    .Categories
                    .FirstOrDefault(c => c.Id == a.CategoryId)
                    .Name,
            })
            .FirstOrDefaultAsync();

        return article;
    }

    /// <summary>
    /// Service for changing(editing) an Article.
    /// </summary>
    /// <param name="model"></param>
    public async Task EditArticleAsync(ArticleEditViewModel model)
    {
        Article editedArticle = new Article
        {
            Id = model.Id,
            Title = model.Title,
            Content = model.Content,
            ModifiedOn = DateTime.Now,
            CategoryId = model.CategoryId,
            Author = model.Author,
        };

        this.repository.Update(editedArticle);
        await this.repository.SaveChangesAsync();
    }

    /// <summary>
    /// Service for deleting an Article from the database.
    /// </summary>
    /// <param name="model"></param>
    public async Task DeleteArticleAsync(ArticleViewModel model)
    {
        Article articleToBeDeleted = await this.context.Articles.FindAsync(model.Id);

        if (articleToBeDeleted != null)
        {
            this.repository.Delete(articleToBeDeleted);
            await this.repository.SaveChangesAsync();
        }
    }
}
