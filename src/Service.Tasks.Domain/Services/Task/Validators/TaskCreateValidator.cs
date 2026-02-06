using FluentValidation;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Domain.Models;
using Service.Tasks.Domain.Models.Base.Validators;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.Domain.Services.Task.Validators;

public class TaskCreateValidator
    : AbstractValidator<TaskModel>,
        IDomainCreateValidator<TaskModel>
{
    public TaskCreateValidator(
        ITaskRepository repository)
    {
        RuleFor(x => x)
            .CustomAsync(async (
                task,
                context,
                token) =>
            {
                var taskExists = (await repository.Get(new FilterSettings { SearchText = $"Title=={task.Title}" },
                    cancellationToken: token)).Any();

                if (taskExists)
                {
                    context.AddFailure(nameof(TaskModel), "Title already exists");
                }
            });
    }
}
