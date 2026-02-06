using AutoMapper;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models;
using Service.Tasks.Domain.Models.Base.Validators;
using Service.Tasks.Domain.Services.Base;

namespace Service.Tasks.Domain.Services.Task;

public class TaskManager
    : DataManagerBase<TaskModel, TaskEntity, ITaskRepository>,
        ITaskManager
{
    private readonly ITransactionService _transactionService;

    public TaskManager(
        IMapper mapper,
        ITaskRepository repository,
        IEnumerable<IDomainValidator<TaskModel>> validators,
        ITransactionService transactionService)
        : base(mapper, repository, validators)
    {
        _transactionService = transactionService;
    }

    protected override async Task<TaskModel> UpdateAction<TDomainUpdate>(
        TDomainUpdate model,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        if (model is not TaskModel taskModel)
        {
            return await base.UpdateAction(model, transaction, cancellationToken);
        }

        return await _transactionService.Execute(async (
            tr,
            token) =>
        {
            foreach (var child in taskModel.Children)
            {
                var taskChild = await Repository.GetOneById(child.Id, transaction: tr, cancellationToken: token);

                taskChild.ParentId = taskModel.Id;

                await base.UpdateAction(taskChild, tr, token);
            }

            return await base.UpdateAction(taskModel, tr, token);
        }, transaction, cancellationToken);
    }
}
