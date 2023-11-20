﻿using System;
using System.Collections.Generic;
using Humanizer;
using Humanizer.Localisation;

namespace TechSolveHR.Models;

public class Employee
{
    public virtual EmergencyContact Contacts { get; set; } = new();

    public virtual Employee? Manager { get; set; }

    public Guid Id { get; set; }

    public Guid? ManagerId { get; set; }

    public virtual List<Attendance> Attendances { get; set; } = new();

    public virtual List<Leave> Leaves { get; set; } = new();

    public virtual PersonalInformation Data { get; set; } = new();

    public string Password { get; set; } = null!;

    public string Status { get; set; } = "Active";

    public string Username { get; set; } = null!;

    public string? AccessType { get; set; }

    public string? CompanyId { get; set; }

    public string? Department { get; set; }

    public string? Division { get; set; }

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

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee Evaluator { get; set; } = null!;

    public Guid EmployeeId { get; set; }

    public Guid EvaluatorId { get; set; }

    public Guid Id { get; set; }

    public int? Rating { get; set; }

    public string? Category { get; set; }

    public string? Feedback { get; set; }

    public string? Title { get; set; }
}

public class Attendance
{
    public DateTimeOffset TimeIn { get; set; }

    public DateTimeOffset TimeOut { get; set; }

    public string TimeInString => TimeIn.Humanize();

    public string TimeOutString => TimeOut.Humanize();

    public Guid Id { get; set; }

    public string LengthString => Length.Humanize();

    public string OverTimeString => OverTime < TimeSpan.Zero ? "None" : OverTime.Humanize(minUnit: TimeUnit.Minute);

    public TimeSpan Length => TimeOut - TimeIn;

    public TimeSpan OverTime
    {
        get
        {
            var timeSpan = Length.Subtract(TimeSpan.FromHours(8));
            return timeSpan > TimeSpan.Zero ? timeSpan : TimeSpan.Zero;
        }
    }
}

public class Address
{
    public Guid Id { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Street { get; set; }

    public string? ZipCode { get; set; }
}

public class PersonalInformation
{
    public virtual Address Address { get; set; } = new();

    public DateTimeOffset? DateOfBirth { get; set; }

    public DateTimeOffset? DateOfHire { get; set; }

    public Guid Id { get; set; }

    public string FullName => $"{FirstName} {MiddleName} {LastName}";

    public string Name => $"{FirstName} {LastName}";

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