using FastFood.Services.Data;

namespace FastFood.Web.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private IEmployeeService employeeService;
        public EmployeesController(IEmployeeService service)
        {
            this.employeeService = service;
        }

        public async Task<IActionResult> Register()
        {
            var positions = await this.employeeService.Register();
            return View(positions);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }
            
            await this.employeeService.Register(model);

            return RedirectToAction("All", "Employees");
        }

        public async Task<IActionResult> All()
        {
            var employees = await this.employeeService.AllAsync();
            return View(employees);
        }
    }
}
