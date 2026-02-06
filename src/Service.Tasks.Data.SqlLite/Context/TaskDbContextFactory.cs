using Microsoft.Extensions.Configuration;

namespace Service.Tasks.Data.SqlLite.Context;

internal class TaskDbContextFactory : SqlLiteDbContextFactoryBase<TaskDbContext>
{
    public TaskDbContextFactory(
        IConfiguration configuration)
        : base(configuration)
    {
    }

    protected override string ConnectionString => "ServiceDB";
}
