using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories.Base;
using Sieve.Services;

namespace Service.Tasks.Data.Repositories;

public abstract class UserRepository<TDbContext>
    : RepositoryBase<UserRepository<TDbContext>, TDbContext, UserEntity>,
        IUserRepository
    where TDbContext : DbContext
{
    protected UserRepository(
        TDbContext dbContext,
        ILogger<UserRepository<TDbContext>> logger,
        ISieveProcessor sieveProcessor)
        : base(dbContext, logger, sieveProcessor)
    {
    }
}
