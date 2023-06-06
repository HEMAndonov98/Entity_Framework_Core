using PetStore.Web.ViewModels.Categories;
using PetStore.Web.ViewModels.Products;

namespace PetStore.Services.Data;

public interface IProductService
{
    public Task CreateAsync(CreateProductInputModel inputModel);

    public Task<IEnumerable<ProductViewModel>> GetAllAsync();

    public Task<IEnumerable<CategoryListViewModel>> GetAllCategoriesAsync();

    public Task DeleteAsync(string id);
}