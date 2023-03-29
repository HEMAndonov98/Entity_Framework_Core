
using FastFood.Web.ViewModels.Items;

namespace FastFood.Services.Data;

public interface IItemsService
{
    public Task<IList<CreateItemViewModel>> CreateAsync();

    public Task CreateAsync(CreateItemInputModel inputModel);

    public Task<IList<ItemsAllViewModels>> AllAsync();
}