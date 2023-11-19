using Microsoft.EntityFrameworkCore;

namespace TechSolveHR.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Employee> Users { get; set; } = null!;
}