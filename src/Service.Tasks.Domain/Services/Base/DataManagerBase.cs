using System.Collections.Immutable;
using AutoMapper;
using FluentValidation;
using Service.Tasks.Data.Models.Base;
using Service.Tasks.Data.Repositories.Base;
using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Domain.Models.Base.Validators;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Service.Tasks.Domain.Services.Base;

public abstract class DataManagerBase<TDomain, TEntity, TRepository> : IDataManager<TDomain>
    where TDomain : class, IModel
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>
{
    protected DataManagerBase(
        IMapper mapper,
        TRepository repository,
        IEnumerable<IDomainValidator<TDomain>> validators)
    {
        Mapper = mapper;
        Repository = repository;
        Validators = validators;
    }

    protected IEnumerable<IDomainValidator<TDomain>> Validators { get; }

    protected IMapper Mapper { get; }
    protected TRepository Repository { get; }

    public async Task<TDomain> Create<TDomainCreate>(
        TDomainCreate model,
        CancellationToken cancellationToken = default)
        where TDomainCreate : class
    {
        Validate<IDomainCreateValidator<TDomain>>(Mapper.Map<TDomain>(model), cancellationToken);
        return await CreateAction(model, cancellationToken);
    }

    protected virtual async Task<TDomain> CreateAction<TDomainCreate>(
        TDomainCreate model,
        CancellationToken cancellationToken = default)
    {
        var entity = Mapper.Map<TEntity>(model);
        var createdEntity = await Repository.Create(entity, cancellationToken);
        return Mapper.Map<TDomain>(createdEntity);
    }

    public async Task<TDomain> Update<TDomainUpdate>(
        TDomainUpdate model,
        CancellationToken cancellationToken = default)
        where TDomainUpdate : class
    {
        var domainModel = Mapper.Map<TDomain>(model);
        Validate<IDomainUpdateValidator<TDomain>>(domainModel, cancellationToken);
        return await UpdateAction(domainModel, cancellationToken);
    }

    protected virtual async Task<TDomain> UpdateAction<TDomainUpdate>(
        TDomainUpdate model,
        CancellationToken cancellationToken = default)
    {
        var entity = Mapper.Map<TEntity>(model);
        var updatedEntity = await Repository.Update(entity, cancellationToken);
        return Mapper.Map<TDomain>(updatedEntity);
    }

    public async Task<TDomain> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetOneById(id, cancellationToken: cancellationToken);
        var domainModel = Mapper.Map<TDomain>(entity);
        Validate<IDomainUpdateValidator<TDomain>>(domainModel, cancellationToken);
        return await DeleteAction(id, cancellationToken);
    }

    protected virtual async Task<TDomain> DeleteAction(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var deletedEntity = await Repository.Delete(id, cancellationToken);
        return Mapper.Map<TDomain>(deletedEntity);
    }

    protected void Validate<TV>(
        TDomain model,
        CancellationToken cancellationToken = default)
        where TV : IDomainValidator<TDomain>
    {
        var validators = Validators.Where(v => v is TV)
            .Cast<IValidator<TDomain>>()
            .ToList();

        Validate(model, validators, cancellationToken);
    }

    private static void Validate<TPayload>(
        TPayload model,
        IEnumerable<IValidator<TPayload>> source,
        CancellationToken cancellationToken = default)
        where TPayload : IModel
    {
        var failures = source.Select(async x => await x.ValidateAsync(model, cancellationToken))
            .SelectMany(x => x.Result.Errors)
            .Where(x => x != null)
            .ToImmutableList();

        if (!failures.IsEmpty)
        {
            throw new ValidationException(failures.ToString());
        }
    }
}
