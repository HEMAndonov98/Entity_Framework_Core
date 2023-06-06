using Microsoft.AspNetCore.Mvc;
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
        
        return RedirectToAction("All");
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        IEnumerable<ProductViewModel> products = await this._productService.GetAllAsync();
        return View(products);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var productId = id.ToString();
        await this._productService.DeleteAsync(productId);

        return RedirectToAction("All");
    }
}