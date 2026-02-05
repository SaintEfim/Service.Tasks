using Microsoft.EntityFrameworkCore;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.SqlLite.Context;
using Sieve.Services;

namespace Service.Tasks.Data.SqlLite.Repositories;

public class ExecutorRepository : ExecutorRepository<DbContext>
{
    public ExecutorRepository(
        TaskDbContext dbContext,
        ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
    }
}
