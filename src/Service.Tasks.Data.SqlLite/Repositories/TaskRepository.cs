using Microsoft.EntityFrameworkCore;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.SqlLite.Context;
using Sieve.Services;

namespace Service.Tasks.Data.SqlLite.Repositories;

public class TaskRepository : TaskRepository<DbContext>
{
    public TaskRepository(
        TaskDbContext dbContext,
        ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
    }
}
