using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.Base;

namespace Service.Tasks.Domain.Services.Base;

public interface IDataManager<TDomain>
    where TDomain : class, IModel
{
    Task<TDomain> Create<TDomainCreate>(
        TDomainCreate entity,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
        where TDomainCreate : class;

    Task<TDomain> Update<TDomainUpdate>(
        TDomainUpdate model,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
        where TDomainUpdate : class;

    Task<TDomain> Delete(
        Guid id,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);
}
