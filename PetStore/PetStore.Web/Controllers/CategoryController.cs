using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetStore.Services.Data;
using PetStore.Web.ViewModels.Categories;

namespace PetStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        public async Task<IActionResult> All()
        {
            var list = new List<CategoryListViewModel>();
            var allCategories = await this._categoryService.GetAll();

            if (allCategories == null)
            {
                list = allCategories.ToList();
            }
            
            return View(allCategories);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Category/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            await this._categoryService.Create(inputModel);

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await this._categoryService.Get(id);
            return View(item);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryListViewModel viewModel)
        {
            await this._categoryService.Edit(viewModel);
            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this._categoryService.Delete(id);
            return RedirectToAction("All");
        }
    }
}