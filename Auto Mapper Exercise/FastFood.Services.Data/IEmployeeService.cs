using FastFood.Web.ViewModels.Employees;

namespace FastFood.Services.Data;

public interface IEmployeeService
{
    public Task<IList<RegisterEmployeeViewModel>> Register();
    public Task Register(RegisterEmployeeInputModel inputModel);

    public Task<IList<EmployeesAllViewModel>> AllAsync();
}