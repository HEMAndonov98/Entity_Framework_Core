using System.Threading.Tasks;
using AspNetCoreTemplate.Web.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTemplate.Web.Controllers;

public class ArticleController : Controller, IArticleController
{
    public Task<IActionResult> All()
    {
        return View();
    }

    public async Task<IActionResult> Add()
    {
        throw new System.NotImplementedException();
    }

    public IActionResult Details()
    {
        throw new System.NotImplementedException();
    }

    public async Task<IActionResult> Edit()
    {
        throw new System.NotImplementedException();
    }

    public async Task<IActionResult> Delete()
    {
        throw new System.NotImplementedException();
    }
}