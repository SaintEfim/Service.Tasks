using AutoMapper;
using Service.Tasks.Data.Models.Base;
using Service.Tasks.Data.Repositories.Base;
using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.Domain.Services.Base;

internal abstract class DataProviderBase<TModel, TEntity, TRepository> : IDataProvider<TModel>
    where TModel : class, IModel
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>
{
    protected DataProviderBase(
        IMapper mapper,
        TRepository repository)
    {
        Mapper = mapper;
        Repository = repository;
    }

    protected IMapper Mapper { get; }
    protected TRepository Repository { get; }

    public virtual async Task<IEnumerable<TModel>> Get(
        FilterSettings? filter = null,
        bool withInclude = false,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        return Mapper.Map<IEnumerable<TModel>>(
            await Repository.Get(filter, withInclude, transaction, cancellationToken));
    }

    public virtual async Task<TModel> GetOneById(
        Guid id,
        bool withInclude = false,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        return Mapper.Map<TModel>(await Repository.GetOneById(id, withInclude, transaction, cancellationToken));
    }
}
