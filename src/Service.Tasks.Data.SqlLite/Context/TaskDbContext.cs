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

    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
