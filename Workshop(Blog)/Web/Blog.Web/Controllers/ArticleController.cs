using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreTemplate.Services.Contracts;
using AspNetCoreTemplate.Web.ViewModels.Category;
using Blog.Web.ViewModels.Article;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTemplate.Web.Controllers;

public class ArticleController : Controller
{
    private readonly ICategoryService categoryService;
    private readonly IArticleService articleService;

    public ArticleController(ICategoryService categoryService, IArticleService articleService)
    {
        this.categoryService = categoryService;
        this.articleService = articleService;
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        List<CategoryViewModel> categories = this.categoryService.GetAllNotTracking()
            .ToList();

        ArticleAddViewModel article = new ArticleAddViewModel()
        {
            Categories = categories,
        };

        return View(article);
    }

    public IActionResult All()
    {
        throw new NotImplementedException();
    }
}