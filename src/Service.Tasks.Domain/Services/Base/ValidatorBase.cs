using System.Collections.Immutable;
using FluentValidation;
using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Domain.Models.Base.Validators;

namespace Service.Tasks.Domain.Services.Base;

public abstract class ValidatorBase<TDomain>
    where TDomain : class, IModel
{
    protected ValidatorBase(
        IEnumerable<IDomainValidator<TDomain>> validators)
    {
        Validators = validators;
    }

    protected IEnumerable<IDomainValidator<TDomain>> Validators { get; }

    protected void Validate<TV>(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        var source = Validators.Where(v => v is IValidator<TDomain> && v.GetType()
                .IsAssignableTo(typeof(TV)))
            .Cast<IValidator<TDomain>>();

        Validate(model, source, cancellationToken);
    }

    protected void Validate<TPayload, TV>(
        TPayload model,
        CancellationToken cancellationToken = default)
        where TPayload : class, IModel
    {
        var source = Validators.Where(v => v is IValidator<TPayload> && v.GetType()
                .IsAssignableTo(typeof(TV)))
            .Cast<IValidator<TPayload>>();

        Validate(model, source, cancellationToken);
    }

    protected void Validate<TPayload>(
        TPayload model,
        string actionName,
        CancellationToken cancellationToken = default)
        where TPayload : class, IModel
    {
        var source = Validators.Where(v =>
                v is IDomainCustomValidator customValidator && customValidator.ActionName == actionName)
            .Cast<IValidator<TPayload>>();

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
            throw new ValidationException(failures);
        }
    }
}
