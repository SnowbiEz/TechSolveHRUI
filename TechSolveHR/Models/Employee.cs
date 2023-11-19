using System;
using System.Collections.Generic;

namespace TechSolveHR.Models;

public class Employee
{
    public virtual Department? Department { get; set; }

    public virtual Division? Division { get; set; }

    public virtual Employee? Manager { get; set; }

    public Guid Id { get; set; }

    public virtual List<Attendance> Attendances { get; set; } = new();

    public virtual List<Leave> Leaves { get; set; } = new();

    public virtual List<Performance> Performances { get; set; } = new();

    public virtual PersonalInformation Data { get; set; } = new();

    public virtual EmergencyContact Contacts { get; set; } = new();

    public string Password { get; set; } = null!;

    public string Status { get; set; } = "Active";

    public string Username { get; set; } = null!;

    public string? CompanyId { get; set; }

    public string? EmailAddress { get; set; }

    public string? Title { get; set; }
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
    public DateTimeOffset DateTime { get; set; }

    public virtual Employee Evaluator { get; set; } = null!;

    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Category { get; set; }

    public string? Feedback { get; set; }

    public string? Grade { get; set; }
}

public class Attendance
{
    public DateTimeOffset TimeIn { get; set; }

    public DateTimeOffset TimeOut { get; set; }

    public Guid Id { get; set; }

    public TimeSpan Length => TimeOut - TimeIn;
}

public class Address
{
    public Guid Id { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? StreetAddress { get; set; }

    public string? ZipCode { get; set; }
}

public class Department
{
    public Guid Id { get; set; }

    public virtual List<Employee> Employees { get; set; } = new();

    public string Name { get; set; } = null!;
}

public class Division
{
    public Guid Id { get; set; }

    public virtual List<Employee> Employees { get; set; } = new();

    public string Name { get; set; } = null!;
}

public class PersonalInformation
{
    public virtual Address Address { get; set; } = new();

    public DateTimeOffset? DateOfBirth { get; set; }

    public DateTimeOffset? DateOfHire { get; set; }

    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? Gender { get; set; }

    public string? LastName { get; set; }

    public string? MaritalStatus { get; set; }

    public string? MiddleName { get; set; }

    public string? PhilHealth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PreferredName { get; set; }

    public string? Sss { get; set; }

    public string? TelephoneNumber { get; set; }

    public string? Tin { get; set; }
}

public class EmergencyContact
{
    public Guid Id { get; set; }
    public string? ContactName { get; set; }
    public string? ContactNumber { get; set; }
    public string? Relationship { get; set; }
}