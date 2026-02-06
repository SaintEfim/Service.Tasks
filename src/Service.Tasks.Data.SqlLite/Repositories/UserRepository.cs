using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.SqlLite.Context;
using Sieve.Services;

namespace Service.Tasks.Data.SqlLite.Repositories;

public class UserRepository : UserRepository<DbContext>
{
    public UserRepository(
        TaskDbContext dbContext,
        ISieveProcessor sieveProcessor,
        ILogger<UserRepository> logger)
        : base(dbContext, logger, sieveProcessor)
    {
    }
}
