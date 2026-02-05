using FluentValidation;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Domain.Models;
using Service.Tasks.Domain.Models.Base.Validators;

namespace Service.Tasks.Domain.Services.Task.Validators;

internal sealed class TaskDeleteValidator
    : AbstractValidator<TaskModel>,
        IDomainDeleteValidator<TaskModel>
{
    public TaskDeleteValidator(
        ITaskRepository taskRepository)
    {
        RuleFor(x => x.Id)
            .CustomAsync(async (
                id,
                context,
                cancellationToken) =>
            {
                var task = await taskRepository.HasChildren(id, cancellationToken: cancellationToken);

                if (task)
                {
                    context.AddFailure("The task has subtasks");
                }
            });
    }
}
