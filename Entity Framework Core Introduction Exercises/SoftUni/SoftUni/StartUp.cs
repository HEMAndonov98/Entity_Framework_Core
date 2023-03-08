﻿using System.Text;
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
            Console.WriteLine(GetEmployeesInPeriod(context));
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
}