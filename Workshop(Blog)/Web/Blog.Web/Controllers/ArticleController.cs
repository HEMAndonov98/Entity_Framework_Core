namespace AspNetCoreTemplate.Web.Controllers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Services.Contracts;
using ViewModels.Category;

using Blog.Web.ViewModels.Article;
using Blog.Web.ViewModels.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Controller for all events related to Articles.
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
    /// Retrieves all Categories and returns a View for adding a new article to the user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Add()
    {
        try
        {
            List<CategoryViewModel> categories = this.categoryService.GetAllNotTracking()
                .ToList();
            ArticleAddViewModel article = new ArticleAddViewModel
            {
                Categories = categories,
            };

            return this.View(article);
        }
        catch (Exception)
        {
            this.logger.LogError("ArticleController/Add/HttpGet");
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
    }

    /// <summary>
    /// Adds the model to the Db and returns the list of all articles.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Add(ArticleAddViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            List<CategoryViewModel> categories = this.categoryService.GetAllNotTracking()
                .ToList();
            model.Categories = categories;
            return View(model);
        }

        try
        {
            var userIdentity = this.HttpContext.User.Identity;
            if (userIdentity != null)
            {
                string author = userIdentity.Name;

                if (!author.IsNullOrEmpty())
                {
                    model.Author = author;
                    await this.articleService.AddArticle(model);

                    return this.RedirectToAction("All");
                }
            }

            throw new ArgumentException();
        }
        catch (ArgumentNullException e)
        {
            this.logger.LogError("ArticleController/Add/HttpPost(NullException)", e);
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
        catch (Exception)
        {
            this.logger.LogError("ArticleController/Add/HttpPost");
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
    }

    /// <summary>
    /// Retrieves all articles and return a view to the user with the result.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> All()
    {
        try
        {
            var articles = await this.articleService.GetAllAsync();

            return this.View(articles);
        }
        catch (Exception)
        {
            this.logger.LogError("ArticleController/All");
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
    }

    /// <summary>
    /// Retrieves Article and shows the user detailed information about it.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var article = await this.articleService.GetArticleAsync(id);

            return this.View(article);
        }
        catch (Exception)
        {
            this.logger.LogError("ArticleController/Details");
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
    }

    /// <summary>
    /// Retrieves the Article to be edited and shows it to the User.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var article = await this.articleService.GetArticleAsync(id);
            ICollection<CategoryViewModel> categories = this.categoryService.GetAllNotTracking();

            ArticleEditViewModel articleEditModel = new ArticleEditViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Author = article.Author,
                Categories = categories,
                CreatedOn = article.CreatedOn,
            };

            return this.View(articleEditModel);
        }
        catch (Exception)
        {
            this.logger.LogError("ArticleController/Edit/HttpGet");
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
    }

    /// <summary>
    /// Updates the database with the edited Article data.
    /// </summary>
    /// <param name="article"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Edit(ArticleEditViewModel article)
    {

        if (!this.ModelState.IsValid)
        {
            ICollection<CategoryViewModel> categories = this.categoryService.GetAllNotTracking();
            article.Categories = categories;
            return this.View(article);
        }

        try
        {
            if (this.HttpContext.User.Identity != null)
            {
                string author = this.HttpContext.User.Identity.Name;
                article.Author = author;
            }

            await this.articleService.EditArticleAsync(article);

            return this.RedirectToAction("All");
        }
        catch (Exception)
        {
            this.logger.LogError("ArticleController/Edit/HttpPost");
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
    }

    /// <summary>
    /// Shows the User an interface for confirming the deletion of his article.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            ArticleViewModel article = await this.articleService.GetArticleAsync(id);

            return this.View(article);
        }
        catch (Exception)
        {
            this.logger.LogError("ArticleController/Delete/HttpGet");
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
    }

    /// <summary>
    /// Deletes the article from the database and return the user to the main page.
    /// </summary>
    /// <param name="articleToBeDeleted"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Delete(ArticleViewModel articleToBeDeleted)
    {
        try
        {
            await this.articleService.DeleteArticleAsync(articleToBeDeleted.Id);

            return this.RedirectToAction("All");
        }
        catch (Exception)
        {
            this.logger.LogError("ArticleController/Delete/HttpPost");
            return this.View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }
    }
}
