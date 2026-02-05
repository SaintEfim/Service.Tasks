using AutoMapper;
using FluentValidation;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Domain.Models;
using Service.Tasks.Domain.Models.Base.Validators;
using Service.Tasks.Domain.Services.Base;

namespace Service.Tasks.Domain.Services.Task;

public class TaskManager
    : DataManagerBase<TaskModel, TaskEntity, ITaskRepository>,
        ITaskManager
{
    public TaskManager(
        IMapper mapper,
        ITaskRepository repository,
        IEnumerable<IDomainValidator<TaskModel>> validators)
        : base(mapper, repository, validators)
    {
    }
}
