using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Web.ViewModels.Employees;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data;

public class EmployeeService : IEmployeeService
{
    private readonly IMapper mapper;
    private readonly FastFoodContext context;
    
    public EmployeeService(IMapper mapper, FastFoodContext context)
    {
        this.mapper = mapper;
        this.context = context;
    }

    public async Task<IList<RegisterEmployeeViewModel>> Register()
    {
        var positions = await this.mapper
            .ProjectTo<RegisterEmployeeViewModel>(this.context.Positions)
            .ToListAsync();

        return positions;
    }

    public async Task Register(RegisterEmployeeInputModel inputModel)
    {
        inputModel.PositionName = this.context.Positions
            .FindAsync(inputModel.PositionId).Result.Name;
        
        Employee employee = this.mapper.Map<Employee>(inputModel);

        await this.context.Employees.AddAsync(employee);
        await this.context.SaveChangesAsync();
    }

    public async Task<IList<EmployeesAllViewModel>> AllAsync()
        => await this.context.Employees
            .Include(e => e.Position)
            .ProjectTo<EmployeesAllViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();
}