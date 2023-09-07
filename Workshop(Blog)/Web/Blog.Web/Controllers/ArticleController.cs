namespace AspNetCoreTemplate.Web.Controllers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using AspNetCoreTemplate.Services.Contracts;
using AspNetCoreTemplate.Web.ViewModels.Category;
using Blog.Web.ViewModels.Article;
using Blog.Web.ViewModels.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
/// <summary>
/// Controller for all events related to Articles
/// </summary>
public class ArticleController : Controller
{
    private readonly ICategoryService categoryService;
    private readonly IArticleService articleService;
    private readonly ILogger logger;

    public ArticleController(ICategoryService categoryService, IArticleService articleService, ILogger<ArticleController> logger)
    {
        this.categoryService = categoryService;
        this.articleService = articleService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all Categories and returns a View for adding a new article to the user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Add()
    {
        try
        {
            List<CategoryViewModel> categories = this.categoryService.GetAllNotTracking()
                .ToList();
            ArticleAddViewModel article = new ArticleAddViewModel()
            {
                Categories = categories,
            };

            return View(article);
        }
        catch (Exception e)
        {
            this.logger.LogError("ArticleController/Add/HttpGet", e);
            return View("Error", new ErrorViewModel()
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Adds the model to the Db and returns the list of all articles
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Add(ArticleAddViewModel model)
    {
        string author = HttpContext.User.Identity.Name;
        
        try
        {
            model.Author = author;
            await this.articleService.AddArticle(model);

            return RedirectToAction("All");
        }
        catch (Exception e)
        {
            this.logger.LogError("ArticleController/Add/HttpPost", e);
            return View("Error", new ErrorViewModel()
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Retrieves all articles and return a view to the user with the result
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> All()
    {
        IEnumerable<ArticleViewModel> articles;

        try
        {
            articles = this.articleService.GetAll();

            return View(articles);
        }
        catch (Exception e)
        {
            this.logger.LogError("ArticleController/All", e);
            return View("Error", new ErrorViewModel()
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
