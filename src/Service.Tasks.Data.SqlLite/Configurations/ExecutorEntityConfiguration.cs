using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Tasks.Data.Models;

namespace Service.Tasks.Data.SqlLite.Configurations;

internal sealed class ExecutorEntityConfiguration : IEntityTypeConfiguration<ExecutorEntity>
{
    public void Configure(
        EntityTypeBuilder<ExecutorEntity> builder)
    {
    }
}
