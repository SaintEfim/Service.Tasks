using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Tasks.Data.Models;

namespace Service.Tasks.Data.SqlLite.Configurations;

internal sealed class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(
        EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasIndex(x => x.Title)
            .IsUnique();
    }
}
