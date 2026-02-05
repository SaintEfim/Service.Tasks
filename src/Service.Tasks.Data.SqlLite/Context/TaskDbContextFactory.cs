using Microsoft.Extensions.Configuration;

namespace Service.Tasks.Data.SqlLite.Context;

public class TaskDbContextFactory : SqlLiteDbContextFactoryBase<TaskDbContext>
{
    public TaskDbContextFactory(
        IConfiguration configuration)
        : base(configuration)
    {
    }

    protected override string ConnectionString => "ServiceDB";
}
