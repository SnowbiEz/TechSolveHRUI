using System;
using System.Collections.Generic;
using System.Web.Helpers;
using AutoBogus;

namespace TechSolveHR.Models;

public class ExampleData
{
    public static List<Employee> GenerateEmployees(int count)
    {
        AutoFaker.Configure(builder =>
        {
            builder
                .WithSkip<Guid>()
                .WithSkip<Guid?>();
        });

        var employees = new List<Employee>();

        for (var i = 0; i < count; i++)
        {
            var employee = AutoFaker.Generate<Employee>();

            employee.Password   = Crypto.HashPassword("password");
            employee.AccessType = Random.Shared.Next(0, 2) == 0 ? "Employee" : "Admin";

            employees.Add(employee);
        }

        return employees;
    }
}