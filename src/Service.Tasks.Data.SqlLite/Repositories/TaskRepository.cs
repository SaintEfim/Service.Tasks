using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.SqlLite.Context;
using Sieve.Services;

namespace Service.Tasks.Data.SqlLite.Repositories;

internal sealed class TaskRepository : TaskRepository<DbContext>
{
    public TaskRepository(
        TaskDbContext dbContext,
        ISieveProcessor sieveProcessor,
        ILogger<TaskRepository> logger)
        : base(dbContext, logger, sieveProcessor)
    {
    }
}
