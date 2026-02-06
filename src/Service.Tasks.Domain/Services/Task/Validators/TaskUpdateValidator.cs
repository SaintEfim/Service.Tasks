using FluentValidation;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Domain.Models.Base.Validators;
using Service.Tasks.Domain.Models.Task;
using Service.Tasks.Domain.Services.Task.Validators.Helpers;

namespace Service.Tasks.Domain.Services.Task.Validators;

internal sealed class TaskUpdateValidator
    : AbstractValidator<TaskModel>,
        IDomainUpdateValidator<TaskModel>
{
    public TaskUpdateValidator(
        ITaskRepository repository)
    {
        RuleFor(x => x)
            .CustomAsync(async (
                task,
                context,
                cancellationToken) =>
            {
                if (!task.ParentId.HasValue)
                {
                    return;
                }

                var tasks = (await repository.Get(cancellationToken: cancellationToken)).ToList();

                var vertices = tasks.Select(x => x.Id)
                    .ToHashSet();

                var graph = tasks.Where(s => s.Id != task.Id)
                    .Select(s => (vertex: s.Id,
                        neighbors: s.ParentId != null && vertices.Contains(s.ParentId!.Value)
                            ? new List<Guid> { s.ParentId.Value }
                            : []))
                    .ToDictionary(k => k.vertex, v => v.neighbors);

                graph[task.Id] = task.ParentId != null && vertices.Contains(task.ParentId!.Value)
                    ? [task.ParentId.Value]
                    : [];

                if (GraphUtils.HasCycle(graph.Select(kv => (kv.Key, kv.Value))
                        .Where(x => x.Value.Count > 0), vertices))
                {
                    context.AddFailure("Cycle detected tasks");
                }
            });
    }
}
