using Microsoft.EntityFrameworkCore;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories.Base;
using Service.Tasks.Shared.Models;
using Sieve.Services;

namespace Service.Tasks.Data.Repositories;

public class TaskRepository<TDbContext>
    : RepositoryBase<TDbContext, TaskEntity>,
        ITaskRepository
    where TDbContext : DbContext
{
    protected TaskRepository(
        TDbContext dbContext,
        ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
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
