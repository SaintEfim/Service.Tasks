using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Tasks.Data.Models;

namespace Service.Tasks.Data.SqlLite.Configurations;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(
        EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(x => x.Role)
            .HasConversion<string>();

        builder.HasIndex(x => x.UserName)
            .IsUnique();
    }
}
