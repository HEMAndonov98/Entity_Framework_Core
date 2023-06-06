using PetStore.Web.ViewModels.Categories;

namespace PetStore.Services.Data;

public interface ICategoryService
{
    public Task Create(CreateCategoryInputModel inputModel);

    public Task<IEnumerable<CategoryListViewModel>> GetAll();

    public Task Delete(int id);

    public Task Edit(CategoryListViewModel viewModel);

    public Task<CategoryListViewModel> Get(int id);
}