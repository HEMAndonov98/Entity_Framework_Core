using FastFood.Web.ViewModels.Categories;

namespace FastFood.Services.Data;

public interface ICategoryService
{
    public Task CreateAsync(CreateCategoryInputModel inputModel);

    public Task<IList<CategoryAllViewModel>> AllAsync();
}