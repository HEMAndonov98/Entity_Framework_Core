using System.Text;
using Microsoft.EntityFrameworkCore;

using SoftUni.Data;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();
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
}

