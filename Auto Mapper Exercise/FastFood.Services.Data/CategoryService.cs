using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Web.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data;

public class CategoryService : ICategoryService
{
    private readonly IMapper mapper;
    private readonly FastFoodContext context;

    public CategoryService(IMapper diMapper, FastFoodContext diContext)
    {
        this.mapper = diMapper;
        this.context = diContext;
    }

    public async Task CreateAsync(CreateCategoryInputModel inputModel)
    {
        Category category = this.mapper.Map<Category>(inputModel);

        await this.context.AddAsync(category);
        await this.context.SaveChangesAsync();
    }

    public async Task<IList<CategoryAllViewModel>> AllAsync()
        => await this.context.Categories
            .ProjectTo<CategoryAllViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();
}