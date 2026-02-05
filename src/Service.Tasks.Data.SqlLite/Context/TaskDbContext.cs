using Microsoft.EntityFrameworkCore;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.SqlLite.Configurations;

namespace Service.Tasks.Data.SqlLite.Context;

public sealed class TaskDbContext : DbContext
{
    public TaskDbContext(
        DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }

    public DbSet<ExecutorEntity> Executors { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ExecutorEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TaskEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
