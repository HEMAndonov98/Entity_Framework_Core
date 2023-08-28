using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTemplate.Web.ViewModels.Category;

namespace AspNetCoreTemplate.Services.Contracts;

public interface ICategoryService
{
    ICollection<CategoryViewModel> GetAllNotTracking();
}