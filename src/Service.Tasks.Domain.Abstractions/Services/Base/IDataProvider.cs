using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.Domain.Services.Base;

public interface IDataProvider<TModel>
    where TModel : class, IModel
{
    Task<IEnumerable<TModel>> Get(
        FilterSettings? filter = null,
        bool withInclude = false,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<TModel> GetOneById(
        Guid id,
        bool withInclude = false,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);
}
