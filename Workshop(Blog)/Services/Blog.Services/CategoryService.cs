using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTemplate.Services.Contracts;
using AspNetCoreTemplate.Web.ViewModels.Category;
using Blog.Data.Common.Repositories;
using Blog.Data.Models;

namespace AspNetCoreTemplate.Services;

public class CategoryService : ICategoryService
{
    private IRepository<Category> repository;

    public CategoryService(IRepository<Category> repository)
    {
        this.repository = repository;
    }

    public ICollection<CategoryViewModel> GetAllNotTracking()
    {
        return this.repository.AllAsNoTracking()
            .Select(cat => new CategoryViewModel()
            {
                Id = cat.Id,
                Name = cat.Name
            })
            .ToList();
    }
}