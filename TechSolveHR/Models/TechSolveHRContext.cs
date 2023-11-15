using Microsoft.EntityFrameworkCore;

namespace TechSolveHR.Models;

public class TechSolveHRContext : DbContext
{
    public TechSolveHRContext(DbContextOptions<TechSolveHRContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
}