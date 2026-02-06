using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models;
using Service.Tasks.Domain.Services.Base;

namespace Service.Tasks.Domain.Services.Task;

public interface ITaskProvider : IDataProvider<TaskModel>
{
    Task<IEnumerable<TaskModel>> ExportTree(
        Guid? rootId = null,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);
}
