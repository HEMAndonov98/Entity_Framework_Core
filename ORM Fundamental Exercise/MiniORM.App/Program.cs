using Data.Entities;

namespace MiniORM.App;
class Program
{
    static void Main(string[] args)
    {
        var connectionString = @"Server=localhost;Database=MiniORM;User Id=SA;Password=Password ;TrustServerCertificate=True";

        var context = new SoftUniDbContext(connectionString);

        context.Employees.Add(new Employee
        {
            FirstName = "Gosho",
            LastName = "Inserted",
            DepartmentId = context.Departments.First().Id,
            IsEmployed = true
        });

        var employee = context.Employees.Last();
        employee.FirstName = "Modified";

        context.SaveChanges();
    }
}

