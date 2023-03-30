using FastFood.Services.Data;

namespace FastFood.Web.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService service)
        {
            this.orderService = service;
        }

        public async Task<IActionResult> Create()
        {
            var viewOrder = await this.orderService.CreateAsync();

            return View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            return RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            throw new NotImplementedException();
        }
    }
}
