using System;
using System.Collections.Generic;

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