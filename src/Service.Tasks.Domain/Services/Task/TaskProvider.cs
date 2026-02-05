using AutoMapper;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Domain.Models;
using Service.Tasks.Domain.Services.Base;

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
}
