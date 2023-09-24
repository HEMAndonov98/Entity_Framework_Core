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
    public async Task<IEnumerable<ArticleViewModel>> GetAllAsync()
    {
        var articles = await this.repository.AllIncludingAsNoTracking(a => a.Category);

        var viewModels = articles
            .Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CreatedOn = a.CreatedOn,
                Author = a.Author,
                Category = a.Category.Name,
            });

        return viewModels;
    }

    /// <summary>
    /// Service for retrieving a single Article from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ArticleViewModel> GetArticleAsync(int id)
    {
        var article = await this.repository
            .FindAsyncIncluding(a => a.Category, a => a.Id == id);

        ArticleViewModel articleViewModel = new ArticleViewModel()
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Author = article.Author,
            CreatedOn = article.CreatedOn,
            Category = article.Category.Name,
        };

        return articleViewModel;
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
            CreatedOn = model.CreatedOn,
        };

        this.repository.Update(editedArticle);
        await this.repository.SaveChangesAsync();
    }

    /// <summary>
    /// Service for deleting an Article from the database.
    /// </summary>
    /// <param name="model"></param>
    public async Task DeleteArticleAsync(int id)
    {
        Article? articleToBeDeleted = await this.repository.FindAsyncIncluding(a => a.Category, a => a.Id == id);

        if (articleToBeDeleted != null)
        {
            this.repository.Delete(articleToBeDeleted);
            await this.repository.SaveChangesAsync();
        }
    }
}
