namespace Service.Tasks.Domain.Models.Base.Validators;

public interface IDomainCustomValidator<TDomain> : IDomainValidator<TDomain>
    where TDomain : IModel
{
    string ActionName { get; }
}
