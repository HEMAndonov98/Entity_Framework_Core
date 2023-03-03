﻿using Data.Entities;

namespace MiniORM.App;
class Program
{
    static async Task Main()
    {
        var connectionString =
            @"Server=localhost;Database=MiniORM;User Id=SA;Password=SuperSecretPassWord**;TrustServerCertificate=True";
        
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

        await context.SaveChanges();
    }
}

