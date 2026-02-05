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

    public async Task<TDomain> Create(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        Validate<IDomainCreateValidator<TDomain>>(model, cancellationToken);
        return await CreateAction(model, cancellationToken);
    }

    protected async Task<TDomain> CreateAction(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        return Mapper.Map<TDomain>(await Repository.Create(Mapper.Map<TEntity>(model), cancellationToken));
    }

    public async Task<TDomain> Update(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        Validate<IDomainUpdateValidator<TDomain>>(model, cancellationToken);
        return await UpdateAction(model, cancellationToken);
    }

    protected async Task<TDomain> UpdateAction(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        return Mapper.Map<TDomain>(await Repository.Update(Mapper.Map<TEntity>(model), cancellationToken));
    }

    public async Task<TDomain> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetOneById(id, cancellationToken: cancellationToken);
        Validate<IDomainUpdateValidator<TDomain>>(Mapper.Map<TDomain>(entity), cancellationToken);
        return await DeleteAction(id, cancellationToken);
    }

    protected async Task<TDomain> DeleteAction(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return Mapper.Map<TDomain>(await Repository.Delete(id, cancellationToken));
    }

    protected void Validate<TV>(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        var source = Validators.Where(v => v is IValidator<TDomain> && v.GetType()
                .IsAssignableTo(typeof(TV)))
            .Cast<IValidator<TDomain>>();

        Validate(model, source, cancellationToken);
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
