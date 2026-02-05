using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Domain.Models.Base.Validators;

namespace Service.Tasks.Domain.Services.Base;

public interface IDataManager<TDomain>
    where TDomain : class, IModel
{
    Task<TDomain> Create<TDomainCreate>(
        TDomainCreate entity,
        CancellationToken cancellationToken = default)
        where TDomainCreate : class;

    Task<TDomain> Update<TDomainUpdate>(
        TDomainUpdate model,
        CancellationToken cancellationToken = default)
        where TDomainUpdate : class;

    Task<TDomain> Delete(
        Guid id,
        CancellationToken cancellationToken = default);
}
