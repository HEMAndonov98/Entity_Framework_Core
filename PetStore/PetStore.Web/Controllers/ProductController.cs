using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetStore.Services.Data;
using PetStore.Web.ViewModels.Products;

namespace PetStore.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        this._productService = productService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await this._productService.GetAllCategoriesAsync();
        
        ViewBag.CategoryOptions = categories;
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductInputModel inputModel)
    {
        await this._productService.CreateAsync(inputModel);
        
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> All()
    {
        IEnumerable<ProductViewModel> products = await this._productService.GetAllAsync();
        return View(products);
    }
}