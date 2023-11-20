using System;

namespace TechSolveHR.Models;

public class Leave
{
    public DateTimeOffset End { get; set; }

    public DateTimeOffset Start { get; set; }

    public Guid Id { get; set; }

    public TimeSpan Length => End - Start;
}