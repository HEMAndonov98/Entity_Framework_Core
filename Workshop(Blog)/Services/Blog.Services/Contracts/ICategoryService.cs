using System.Collections.Generic;
using AspNetCoreTemplate.Web.ViewModels.Category;

namespace AspNetCoreTemplate.Services.Contracts;

public interface ICategoryService
{
    ICollection<CategoryViewModel> GetAllNotTracking();
}