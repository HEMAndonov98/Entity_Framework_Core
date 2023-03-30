using System.Globalization;
using FastFood.Models;
using FastFood.Services.Data;

namespace FastFood.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Items;

    public class ItemsController : Controller
    {
        private readonly IItemsService itemsService;

        public ItemsController(IItemsService service)
        {
            this.itemsService = service;
        }

        public async Task<IActionResult> Create()
        {
            var categories = await this.itemsService.CreateAsync();

            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string price, int categoryId)
        {
            CreateItemInputModel inputModel = new CreateItemInputModel()
            {
                Name = name,
                Price = decimal.Parse(price, CultureInfo.InvariantCulture),
                CategoryId = categoryId
            };
            
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }
            
            await this.itemsService.CreateAsync(inputModel);
            return RedirectToAction("All", "Items");
        }

        public async Task<IActionResult> All()
        {
            IList<ItemsAllViewModels> items = await this.itemsService.AllAsync();

            return View(items);
        }
    }
}
