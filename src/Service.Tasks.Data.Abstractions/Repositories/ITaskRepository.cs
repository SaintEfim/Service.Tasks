using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories.Base;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.Data.Repositories;

public interface ITaskRepository : IRepository<TaskEntity>
{
    Task<bool> HasChildren(
        Guid id,
        FilterSettings? filterSettings = null,
        CancellationToken cancellationToken = default);
}
