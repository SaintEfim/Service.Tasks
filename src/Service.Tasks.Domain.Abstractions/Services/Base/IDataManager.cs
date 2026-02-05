using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Domain.Models.Base.Validators;

namespace Service.Tasks.Domain.Services.Base;

public interface IDataManager<TDomain>
    where TDomain : class, IModel
{
    Task<TDomain> Create(
        TDomain model,
        CancellationToken cancellationToken = default);

    Task<TDomain> Update(
        TDomain model,
        CancellationToken cancellationToken = default);

    Task<TDomain> Delete(
        Guid id,
        CancellationToken cancellationToken = default);
}
