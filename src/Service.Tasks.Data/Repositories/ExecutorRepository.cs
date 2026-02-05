using Microsoft.EntityFrameworkCore;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories.Base;
using Sieve.Services;

namespace Service.Tasks.Data.Repositories;

public class ExecutorRepository<TDbContext>
    : RepositoryBase<TDbContext, ExecutorEntity>,
        IExecutorRepository
    where TDbContext : DbContext
{
    protected ExecutorRepository(
        TDbContext dbContext,
        ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
    }
}
