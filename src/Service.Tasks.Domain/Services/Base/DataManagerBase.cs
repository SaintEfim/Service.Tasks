using AutoMapper;
using FluentValidation;
using Service.Tasks.Data.Models.Base;
using Service.Tasks.Data.Repositories.Base;
using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Domain.Models.Base.Validators;

namespace Service.Tasks.Domain.Services.Base;

public abstract class DataManagerBase<TDomain, TEntity, TRepository>
    : ValidatorBase<TDomain>,
        IDataManager<TDomain>
    where TDomain : class, IModel
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>
{
    protected DataManagerBase(
        IMapper mapper,
        TRepository repository,
        IEnumerable<IDomainValidator<TDomain>> validators)
        : base(validators)
    {
        Mapper = mapper;
        Repository = repository;
    }

    protected IMapper Mapper { get; }
    protected TRepository Repository { get; }

    public async Task<TDomain> Create<TDomainCreate>(
        TDomainCreate model,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
        where TDomainCreate : class
    {
        Validate<IDomainCreateValidator<TDomain>>(Mapper.Map<TDomain>(model), cancellationToken);
        return await CreateAction(model, transaction, cancellationToken);
    }

    protected virtual async Task<TDomain> CreateAction<TDomainCreate>(
        TDomainCreate model,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var entity = Mapper.Map<TEntity>(model);
        var createdEntity = await Repository.Create(entity, transaction, cancellationToken);
        return Mapper.Map<TDomain>(createdEntity);
    }

    public async Task<TDomain> Update<TDomainUpdate>(
        TDomainUpdate model,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
        where TDomainUpdate : class
    {
        var domainModel = Mapper.Map<TDomain>(model);
        Validate<IDomainUpdateValidator<TDomain>>(domainModel, cancellationToken);
        return await UpdateAction(domainModel, transaction, cancellationToken);
    }

    protected virtual async Task<TDomain> UpdateAction<TDomainUpdate>(
        TDomainUpdate model,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var entity = Mapper.Map<TEntity>(model);
        var updatedEntity = await Repository.Update(entity, transaction, cancellationToken);
        return Mapper.Map<TDomain>(updatedEntity);
    }

    public async Task<TDomain> Delete(
        Guid id,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetOneById(id, cancellationToken: cancellationToken);
        var domainModel = Mapper.Map<TDomain>(entity);
        Validate<IDomainUpdateValidator<TDomain>>(domainModel, cancellationToken);
        return await DeleteAction(id, transaction, cancellationToken);
    }

    protected virtual async Task<TDomain> DeleteAction(
        Guid id,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var deletedEntity = await Repository.Delete(id, transaction, cancellationToken);
        return Mapper.Map<TDomain>(deletedEntity);
    }
}
