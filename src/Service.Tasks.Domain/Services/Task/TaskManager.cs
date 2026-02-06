using AutoMapper;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.Base.Validators;
using Service.Tasks.Domain.Models.Task;
using Service.Tasks.Domain.Services.Base;
using Service.Tasks.Shared.Models;

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

    protected override Task<TaskModel> DeleteAction(
        Guid id,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        return _transactionService.Execute(async (
            tr,
            cancellation) =>
        {
            var task = await Repository.GetOneById(id, true, tr, cancellation);

            if (task.Children.Count != 0)
            {
                await DeleteChildTask(id, tr, cancellation);
            }

            var deletedModel = await base.DeleteAction(id, tr, cancellation);

            return deletedModel;
        }, transaction, cancellationToken);
    }

    private async System.Threading.Tasks.Task DeleteChildTask(
        Guid parentId,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var childTasks = await Repository.Get(new FilterSettings { SearchText = $"ParentId == {parentId}" }, true,
            transaction: transaction, cancellationToken: cancellationToken);

        foreach (var childTask in childTasks)
        {
            if (childTask.Children.Count != 0)
            {
                await DeleteChildTask(childTask.Id, transaction, cancellationToken);
            }

            await Repository.Delete(childTask.Id, transaction, cancellationToken);
        }
    }
}
