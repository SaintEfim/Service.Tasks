using Microsoft.EntityFrameworkCore;
using Service.Tasks.Shared.Models;
using Service.Tasks.Data.Models.Base;
using Sieve.Models;
using Sieve.Services;

namespace Service.Tasks.Data.Repositories.Base;

public abstract class RepositoryBase<TDbContext, TEntity> : IRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : class, IEntity
{
    protected RepositoryBase(
        TDbContext dbContext,
        ISieveProcessor sieveProcessor)
    {
        DbContext = dbContext;
        SieveProcessor = sieveProcessor;
    }

    private TDbContext DbContext { get; }

    private ISieveProcessor SieveProcessor { get; }

    public virtual async Task<IEnumerable<TEntity>> Get(
        FilterSettings? filter,
        bool withIncludes = false,
        CancellationToken cancellationToken = default)
    {
        var source = BuildBaseQuery(withIncludes);

        if (filter != null)
        {
            var sieve = new SieveModel { Filters = filter.Filter };

            source = SieveProcessor.Apply(sieve, source);
        }

        return await source.ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> GetOneById(
        Guid id,
        bool withIncludes = false,
        CancellationToken cancellationToken = default)
    {
        var entity = await BuildBaseQuery(withIncludes)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException($"{typeof(TEntity)} with id {id} not found.");
        }

        return entity;
    }

    public virtual async Task<TEntity> Create(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        var createdEntity = await DbContext.Set<TEntity>()
            .AddAsync(entity, cancellationToken);

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            // detach entity to prevent
            // "The instance of entity type cannot be tracked because another instance with the key value  is already being tracked."
            createdEntity.State = EntityState.Detached;
        }

        return createdEntity.Entity;
    }

    public virtual async Task<TEntity> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await GetOneById(id, cancellationToken: cancellationToken);

        var deletedEntity = DbContext.Set<TEntity>()
            .Remove(entity);

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            deletedEntity.State = EntityState.Detached;
        }

        return deletedEntity.Entity;
    }

    public virtual async Task<TEntity> Update(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        var updatedRowEntry = DbContext.Set<TEntity>()
            .Update(entity);

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            updatedRowEntry.State = EntityState.Detached;
        }

        return updatedRowEntry.Entity;
    }

    protected virtual IQueryable<TEntity> FillRelatedRecords(
        IQueryable<TEntity> query)
    {
        return query;
    }

    protected virtual IQueryable<TEntity> BuildBaseQuery(
        bool withIncludes)
    {
        var query = DbContext.Set<TEntity>()
            .AsNoTracking();

        if (withIncludes)
        {
            query = FillRelatedRecords(query);
        }

        return query;
    }
}
