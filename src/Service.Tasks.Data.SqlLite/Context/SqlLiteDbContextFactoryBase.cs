using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Service.Tasks.Data.SqlLite.Context;

public class SqlLiteDbContextFactoryBase<TDbContext>
    where TDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    protected SqlLiteDbContextFactoryBase(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected virtual string ConnectionString => string.Empty;

    public TDbContext CreateDbContext()
    {
        var connectionString = _configuration.GetConnectionString(ConnectionString);

        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
        optionsBuilder.UseSqlite(connectionString, options => { options.CommandTimeout(30); });
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();

        return (TDbContext) Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options)!;
    }
}
