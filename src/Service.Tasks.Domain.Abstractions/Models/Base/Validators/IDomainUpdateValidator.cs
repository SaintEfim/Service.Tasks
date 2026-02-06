namespace Service.Tasks.Domain.Models.Base.Validators;

public interface IDomainUpdateValidator<TDomain> : IDomainValidator<TDomain>
    where TDomain : IModel;
