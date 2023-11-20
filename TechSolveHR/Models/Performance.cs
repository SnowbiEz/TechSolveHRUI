using System;

namespace TechSolveHR.Models;

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