using FastFood.Services.Data;

namespace FastFood.Web.Controllers
{
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Positions;

    public class PositionsController : Controller
    {
        private readonly IPositionsService positionsService;
        public PositionsController(IPositionsService positionsService)
        {
            this.positionsService = positionsService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            await this.positionsService.Create(model);

            return RedirectToAction("All", "Positions");
        }

        public async Task<IActionResult> All()
        {
            var positions = await this.positionsService
                .GetAll();

            return View(positions.ToList());
        }
    }
}
