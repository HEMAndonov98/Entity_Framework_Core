using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTemplate.Web.Controllers.Contracts;

public interface IArticleController
{
    Task<IActionResult> All();

    Task<IActionResult> Add();

    IActionResult Details();

    Task<IActionResult> Edit();

    Task<IActionResult> Delete();
}