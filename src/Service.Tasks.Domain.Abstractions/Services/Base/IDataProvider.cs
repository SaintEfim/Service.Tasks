using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Shared.Models;

namespace Service.Task.Domain.Services.Base;

public interface IDataProvider<TModel>
    where TModel : class, IModel
{
    Task<IEnumerable<TModel>> Get(
        FilterSettings? filter = null,
        bool withInclude = false,
        CancellationToken cancellationToken = default);

    Task<TModel> GetOneById(
        Guid id,
        bool withInclude = false,
        CancellationToken cancellationToken = default);
}
