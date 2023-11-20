using System;

namespace TechSolveHR.Models;

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