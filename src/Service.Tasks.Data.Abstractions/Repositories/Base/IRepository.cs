using Service.Tasks.Shared.Models;
using Service.Tasks.Data.Models.Base;

namespace Service.Tasks.Data.Repositories.Base;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<IEnumerable<TEntity>> Get(
        FilterSettings? filter = null,
        bool withIncludes = false,
        CancellationToken cancellationToken = default);

    Task<TEntity> GetOneById(
        Guid id,
        bool withIncludes = false,
        CancellationToken cancellationToken = default);

    Task<TEntity> Create(
        TEntity entity,
        CancellationToken cancellationToken = default);

    Task<TEntity> Delete(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<TEntity> Update(
        TEntity entity,
        CancellationToken cancellationToken = default);
}
