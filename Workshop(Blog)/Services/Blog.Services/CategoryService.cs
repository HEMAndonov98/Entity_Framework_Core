using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTemplate.Services.Contracts;
using AspNetCoreTemplate.Web.ViewModels.Category;
using Blog.Data.Common.Repositories;
using Blog.Data.Models;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTemplate.Services;

public class CategoryService : ICategoryService
{
    private ILogger<Category> logger;
    private IRepository<Category> repository;

    public CategoryService(ILogger<Category> logger, IRepository<Category> repository)
    {
        this.logger = logger;
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