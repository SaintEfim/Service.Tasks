using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories.Base;
using Service.Tasks.Shared.Models;
using Sieve.Services;

namespace Service.Tasks.Data.Repositories;

public abstract class TaskRepository<TDbContext>
    : RepositoryBase<TaskRepository<TDbContext>, TDbContext, TaskEntity>,
        ITaskRepository
    where TDbContext : DbContext
{
    protected TaskRepository(
        TDbContext dbContext,
        ILogger<TaskRepository<TDbContext>> logger,
        ISieveProcessor sieveProcessor)
        : base(dbContext, logger, sieveProcessor)
    {
    }

    protected override IQueryable<TaskEntity> FillRelatedRecords(
        IQueryable<TaskEntity> query)
    {
        return query.Include(x => x.Children)
            .Include(x => x.Parent);
    }

    public async Task<bool> HasChildren(
        Guid id,
        FilterSettings? filterSettings = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildBaseQuery();

        return await query.FirstOrDefaultAsync(x => x.ParentId == id, cancellationToken) != null;
    }
}
