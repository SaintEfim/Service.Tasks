using AutoMapper;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.Task;
using Service.Tasks.Domain.Services.Base;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.Domain.Services.Task;

public class TaskProvider
    : DataProviderBase<TaskModel, TaskEntity, ITaskRepository>,
        ITaskProvider
{
    public TaskProvider(
        IMapper mapper,
        ITaskRepository repository)
        : base(mapper, repository)
    {
    }

    public async Task<IEnumerable<TaskModel>> ExportTree(
        Guid? rootId = null,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var allTasks = new List<TaskModel>();

        if (!rootId.HasValue)
        {
            var rootTasks = await Repository.Get(new FilterSettings { SearchText = "ParentId == null" }, true,
                transaction: transaction, cancellationToken: cancellationToken);

            foreach (var task in rootTasks)
            {
                await CollectAllTasksRecursively(task, allTasks, transaction, cancellationToken);
            }
        }
        else
        {
            var task = await Repository.GetOneById(rootId.Value, true, transaction, cancellationToken);
            await CollectAllTasksRecursively(task, allTasks, transaction, cancellationToken);
        }

        return allTasks;
    }

    private async System.Threading.Tasks.Task CollectAllTasksRecursively(
        TaskEntity entity,
        List<TaskModel> allTasks,
        ITransaction? transaction,
        CancellationToken cancellationToken)
    {
        var model = new TaskModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ParentId = entity.ParentId
        };

        allTasks.Add(model);

        if (entity.Children.Count != 0)
        {
            foreach (var childEntity in entity.Children)
            {
                var fullChildEntity = await Repository.GetOneById(childEntity.Id, true, transaction, cancellationToken);

                await CollectAllTasksRecursively(fullChildEntity, allTasks, transaction, cancellationToken);
            }
        }
    }
}
