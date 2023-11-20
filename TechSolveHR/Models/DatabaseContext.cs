using Microsoft.EntityFrameworkCore;

namespace TechSolveHR.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<Performance> Performances { get; set; } = null!;
}