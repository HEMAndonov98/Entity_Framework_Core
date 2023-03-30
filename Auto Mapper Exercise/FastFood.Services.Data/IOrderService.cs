using FastFood.Web.ViewModels.Orders;

namespace FastFood.Services.Data;

public interface IOrderService
{
    public Task<CreateOrderViewModel> CreateAsync();

    public Task CreateAsync(CreateOrderInputModel inputModel);

    public Task<IList<OrderAllViewModel>> AllAsync();
}