using AutoMapper;
using Kf.Service.Warehouse.Domain.Models.Base;
using Service.Tasks.Data.Models.Base;
using Service.Tasks.Data.Repositories.Base;

namespace Service.Task.Domain.Services.Base;

public abstract class DataManagerBase<TModel, TEntity, TRepository> : IDataManager<TModel>
    where TModel : class, IModel
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>
{
    protected DataManagerBase(
        IMapper mapper,
        TRepository repository)
    {
        Mapper = mapper;
        Repository = repository;
    }

    protected IMapper Mapper { get; }
    protected TRepository Repository { get; }

    public async Task<TModel> Create(
        TModel model,
        CancellationToken cancellationToken = default)
    {
        return Mapper.Map<TModel>(await Repository.Create(Mapper.Map<TEntity>(model), cancellationToken));
    }

    public async Task<TModel> Update(
        TModel model,
        CancellationToken cancellationToken = default)
    {
        return Mapper.Map<TModel>(await Repository.Update(Mapper.Map<TEntity>(model), cancellationToken));
    }

    public async Task<TModel> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return Mapper.Map<TModel>(await Repository.Delete(id, cancellationToken));
    }
}
