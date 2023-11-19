using System;
using System.Collections.Generic;

namespace TechSolveHR.Models;

public class Employee
{
    public Guid Id { get; set; }

    public virtual List<Attendance> Attendances { get; set; } = new();

    public virtual PersonalInformation Data { get; set; } = new();

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? CompanyId { get; set; }
}

public class Leave
{
    public DateTimeOffset End { get; set; }

    public DateTimeOffset Start { get; set; }

    public Guid Id { get; set; }

    public TimeSpan Length => End - Start;
}

public class Performance
{
    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee Evaluator { get; set; } = null!;

    public Guid Id { get; set; }
}

public class Attendance
{
    public DateTimeOffset TimeIn { get; set; }

    public DateTimeOffset TimeOut { get; set; }

    public Guid Id { get; set; }

    public TimeSpan Length => TimeOut - TimeIn;
}

public class PersonalInformation
{
    public DateTimeOffset HireDate { get; set; }

    public DateTimeOffset? DateOfBirth { get; set; }

    public DateTimeOffset? DateOfHire { get; set; }

    public Employee? Manager { get; set; }

    public Guid Id { get; set; }

    public string? Address { get; set; }

    public string? Department { get; set; }

    public string? Division { get; set; }

    public string? EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? Gender { get; set; }

    public string? LastName { get; set; }

    public string? Location { get; set; }

    public string? MaritalStatus { get; set; }

    public string? MiddleName { get; set; }

    public string? PhilHealth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PreferredName { get; set; }

    public string? Role { get; set; }

    public string? Sss { get; set; }

    public string? Tin { get; set; }
}