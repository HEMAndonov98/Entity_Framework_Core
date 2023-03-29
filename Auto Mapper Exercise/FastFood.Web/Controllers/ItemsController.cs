using FastFood.Models;
using FastFood.Services.Data;

namespace FastFood.Web.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
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
        public async Task<IActionResult> Create(CreateItemInputModel model)
        {
            await this.itemsService.CreateAsync(model);
            return RedirectToAction("All", "Items");
        }

        public async Task<IActionResult> All()
        {
            IList<ItemsAllViewModels> items = await this.itemsService.AllAsync();

            return View(items);
        }
    }
}
