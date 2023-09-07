using System.Collections.Generic;
using System.Linq;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

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

    private readonly IRepository<Article> repository;
    private readonly ApplicationDbContext context;
    
    public ArticleService(IRepository<Article> repository, ApplicationDbContext context)
    {
        this.repository = repository;
        this.context = context;
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
            CategoryId = model.CategoryId,
            Author = model.Author,
        };

        await this.repository.AddAsync(newArticle);
        await this.repository.SaveChangesAsync();
    }

    /// <summary>
    /// Service for retrieving all articles from database
    /// </summary>
    /// <returns></returns>
    public  IEnumerable<ArticleViewModel> GetAll()
    {
        var articles = this.repository.All()
            .Select(a => new ArticleViewModel()
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
}