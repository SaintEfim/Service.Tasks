namespace Service.Tasks.Domain.Models.Base.Validators;

public interface IDomainCreateValidator<TDomain> : IDomainValidator<TDomain>
    where TDomain : IModel;
