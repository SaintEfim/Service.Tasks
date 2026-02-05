using AutoMapper;
using Service.Task.Domain.Models;
using Service.Task.Domain.Services.Base;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;

namespace Service.Task.Domain.Services.Task;

public class TaskManager : DataManagerBase<TaskModel, TaskEntity, ITaskRepository>
{
    public TaskManager(
        IMapper mapper,
        ITaskRepository repository)
        : base(mapper, repository)
    {
    }
}
