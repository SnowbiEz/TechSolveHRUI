using System;

namespace TechSolveHR.Models;

public class EmergencyContact
{
    public Guid Id { get; set; }

    public string? ContactName { get; set; }

    public string? ContactNumber { get; set; }

    public string? Relationship { get; set; }
}