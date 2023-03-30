using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Web.ViewModels.Orders;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data;

public class OrderService : IOrderService
{
    private readonly IMapper mapper;
    private readonly FastFoodContext context;
    
    public OrderService(IMapper diMapper, FastFoodContext diContext)
    {
        this.mapper = diMapper;
        this.context = diContext;
    }
    
    public async Task<CreateOrderViewModel> CreateAsync()
    {
        var entityEmployees = await this.context
            .Employees
            .AsNoTracking()
            .Select(e => new
            {
                e.Id,
                e.Name
            })
            .ToListAsync();

        var entityItems = await this.context
            .Items
            .AsNoTracking()
            .Select(i => new
            {
                i.Id,
                i.Name
            })
            .ToListAsync();

        var viewModel = new CreateOrderViewModel();

        viewModel.EmployeesNames = entityEmployees
            .ToDictionary(k => k.Id, v => v.Name);
        viewModel.ItemsNames = entityItems
            .ToDictionary(k => k.Id, v => v.Name);

        return viewModel;
    }

    public async Task CreateAsync(CreateOrderInputModel inputModel)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<OrderAllViewModel>> AllAsync()
    {
        throw new NotImplementedException();
    }
}