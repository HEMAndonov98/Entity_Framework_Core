using FastFood.Services.Data;

namespace FastFood.Web.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService service)
        {
            this.categoryService = service;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }
            
            await this.categoryService.CreateAsync(model);
            return RedirectToAction("All", "Categories");
        }

        public async Task<IActionResult> All()
        {
            var allCategories = await this.categoryService.AllAsync();

            return View(allCategories);
        }
    }
}
