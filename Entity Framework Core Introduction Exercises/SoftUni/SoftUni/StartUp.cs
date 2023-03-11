using System.Text;
using Microsoft.EntityFrameworkCore;

using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        using (SoftUniContext context = new SoftUniContext())
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Console.WriteLine(RemoveTown(context));
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }

    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        var sb = new StringBuilder();
        var employees = context.Employees
            .AsNoTracking()
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToList();

        foreach (var employee in employees)
        {
            sb.AppendLine(
                $"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
        }

        return sb.ToString().Trim();
    }

    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        var sb = new StringBuilder();
        var employees = context.Employees
            .AsNoTracking()
            .Where(e => e.Salary > 50000)
            .Select(e => new
            {
                e.FirstName,
                e.Salary
            })
            .OrderBy(e => e.FirstName)
            .ToList();


        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
        }

        return sb.ToString().Trim();
    }

    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        var sb = new StringBuilder();
        var employeesDepartmentsJoin = context.Departments
            .AsNoTracking()
            .Join(context.Employees,
                d => d.DepartmentId,
                e => e.DepartmentId,
                (d, e) => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Department = d.Name,
                    Salary = e.Salary
                })
            .Where(ed => ed.Department == "Research and Development")
            .OrderBy(ed => ed.Salary)
            .ThenByDescending(ed => ed.FirstName)
            .ToList();

        foreach (var ed in employeesDepartmentsJoin)
        {
            sb.AppendLine($"{ed.FirstName} {ed.LastName} from {ed.Department} - ${ed.Salary:F2}");
        }

        return sb.ToString().Trim();
    }

    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        string searchedEmployeeName = "Nakov";
        string newAddressText = "Vitoshka 15";
        int townId = 4;

        var address = new Address()
        {
            TownId = townId,
            AddressText = newAddressText
        };

        var employee = context.Employees
            .FirstOrDefault(e => e.LastName == searchedEmployeeName);

        employee.Address = address;

        context.SaveChanges();

        var sb = new StringBuilder();
        var addresses = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Select(e => new
            {
                Address = e.Address.AddressText
            })
            .Take(10)
            .ToList();

        foreach (var a in addresses)
        {
            sb.AppendLine(a.Address);
        }

        return sb.ToString().Trim();
    }

    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        var sb = new StringBuilder();
        var employees = context.Employees
            .Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                MFirstName = e.Manager.FirstName,
                MLastName = e.Manager.LastName,
                Projects = e.EmployeesProjects
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ep.Project.StartDate,
                        ep.Project.EndDate
                    })
                    .Where(p => p.StartDate.Year >= 2001 &&
                                p.StartDate.Year <= 2003)
            })
            .ToList();

        var dateTimeFormat = "M/d/yyyy h:mm:ss tt";

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.MFirstName} {e.MLastName}");
            foreach (var p in e.Projects)
                sb.AppendLine(
                    $"--{p.ProjectName} - {p.StartDate.ToString(dateTimeFormat)} - {(p.EndDate.HasValue ? p.EndDate.Value.ToString(dateTimeFormat) : "not finished")}");
        }

        return sb.ToString().Trim();
    }

    public static string GetAddressesByTown(SoftUniContext context)
    {
        var sb = new StringBuilder();
        var addresses = context.Addresses
            .AsNoTracking()
            .OrderByDescending(a => a.Employees.Count)
            .ThenBy(a => a.Town.Name)
            .ThenBy(a => a.AddressText)
            .Take(10)
            .Select(a => new
            {
                a.AddressText,
                TownName = a.Town.Name,
                EmployeeCount = a.Employees.Count
            })
            .ToList();

        foreach (var a in addresses)
            sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");

        return sb.ToString()
            .Trim();
    }

    public static string GetEmployee147(SoftUniContext context)
    {
        var sb = new StringBuilder();
        
        int idToSearch = 147;
        var employee = context.Employees
            .Find(idToSearch);
        var projects = employee.EmployeesProjects
            .Select(ep => new
            {
                Name = ep.Project.Name
            })
            .OrderBy(p => p.Name)
            .ToList();

        sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
        sb.AppendJoin(Environment.NewLine, projects.Select(p => p.Name));

        return sb.ToString()
            .Trim();
    }

    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        var sb = new StringBuilder();
        var dptsWithMoreThan5Employees = context.Departments
            .AsNoTracking()
            .Where(dpt => dpt.Employees.Count > 5)
            .OrderBy(dpt => dpt.Employees.Count)
            .ThenBy(dpt => dpt.Name)
            .Select(dpt => new
            {
                Name = dpt.Name,
                ManagerName = $"{dpt.Manager.FirstName} {dpt.Manager.LastName}",
                Employees = dpt.Employees
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .Select(e => new
                    {
                        Name = $"{e.FirstName} {e.LastName}",
                        JobTitle = e.JobTitle
                    })
            })
            .AsSplitQuery()
            .ToList();

        foreach (var dpt in dptsWithMoreThan5Employees)
        {
            sb.AppendLine($"{dpt.Name} - {dpt.ManagerName}");
            foreach (var e in dpt.Employees)
            {
                sb.AppendLine($"{e.Name} - {e.JobTitle}");
            }
        }

        return sb.ToString()
            .Trim();
    }

    public static string GetLatestProjects(SoftUniContext context)
    {
        var sb = new StringBuilder();

        var latestProjects = context.Projects
            .AsNoTracking()
            .OrderByDescending(p => p.StartDate)
            .Take(10)
            .Select(p => new
            {
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate
            })
            .OrderBy(p => p.Name)
            .ToList();

        foreach (var project in latestProjects)
        {
            sb.AppendLine($"{project.Name}");
            sb.AppendLine($"{project.Description}");
            sb.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt")}");
        }

        return sb.ToString()
            .Trim();
    }

    public static string IncreaseSalaries(SoftUniContext context)
    {
        decimal percentageIncrease = (decimal)1.12;
        var sb = new StringBuilder();
        
        var promotedEmployees = context.Employees
            .Where(e => e.Department.Name.Contains("Engineering") ||
                        e.Department.Name.Contains("Tool Design") ||
                        e.Department.Name.Contains("Marketing") ||
                        e.Department.Name.Contains("Information Services"))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Select(e => new
            {
                FullName = $"{e.FirstName} {e.LastName}",
                Salary = (e.Salary * percentageIncrease)
            });

        context.SaveChanges();

        foreach (var employee in promotedEmployees)
        {
            sb.AppendLine($"{employee.FullName} (${employee.Salary:F2})");
        }

        return sb.ToString()
            .Trim();
    }

    public  static  string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        var sb = new StringBuilder();
        var saEmployees = context.Employees
            .AsNoTracking()
            .Where(e => e.FirstName.ToLower().StartsWith("sa"))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Select(e => new
            {
                FullName = $"{e.FirstName} {e.LastName}",
                JobTitle = e.JobTitle,
                Salary = $"${e.Salary:F2}"
            })
            .ToList();

        foreach (var employee in saEmployees)
        {
            sb.AppendLine($"{employee.FullName} - {employee.JobTitle} - ({employee.Salary})");
        }

        return sb.ToString()
            .Trim();
    }

    public static string DeleteProjectById(SoftUniContext context)
    {
        var sb = new StringBuilder();
        var projectToDelete = context.Projects
            .Find(2);

        var projectEmployees = context.EmployeesProjects
            .Where(ep => ep.ProjectId == 2);

        foreach (var ep in projectEmployees)
        {
            context.Remove(ep);
        }

        context.Remove(projectToDelete);
        context.SaveChanges();

        sb.AppendJoin(Environment.NewLine,
            context.Projects
                .Take(10)
                .Select(p => p.Name));
        
        return sb.ToString()
            .Trim();
    }

    public static string RemoveTown(SoftUniContext context)
    {
        var townToDelete = context.Towns
            .FirstOrDefault(t => t.Name == "Seattle");
        
        //Note if in real example you should put if statement to check if the town actually exists

        var addressesToDelete = context.Addresses
            .Where(a => a.Town == townToDelete);

        int deletedAddressesCount = addressesToDelete.Count();

        var employees = context.Employees
            .Include(e => e.Address)
            .Where(e => e.Address.Town == townToDelete)
            .ToList();

        foreach (var employee in employees)
        {
            employee.Address = null;
        }
        
        context.Addresses.RemoveRange(addressesToDelete);
        context.Towns.Remove(townToDelete);
        context.SaveChanges();

        return $"{deletedAddressesCount} addresses in Seattle were deleted";
    }
}