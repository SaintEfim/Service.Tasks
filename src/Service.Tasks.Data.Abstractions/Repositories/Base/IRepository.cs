using Service.Tasks.Shared.Models;
using Service.Tasks.Data.Models.Base;
using Service.Tasks.Data.Services;

namespace Service.Tasks.Data.Repositories.Base;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<IEnumerable<TEntity>> Get(
        FilterSettings? filter = null,
        bool withIncludes = false,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<TEntity> GetOneById(
        Guid id,
        bool withIncludes = false,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<TEntity> Create(
        TEntity entity,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<TEntity> Delete(
        Guid id,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<TEntity> Update(
        TEntity entity,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);
}
