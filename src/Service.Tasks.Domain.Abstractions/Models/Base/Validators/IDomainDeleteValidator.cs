namespace Service.Tasks.Domain.Models.Base.Validators;

public interface IDomainDeleteValidator<TDomain> : IDomainValidator<TDomain>
    where TDomain : IModel;
