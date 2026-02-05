using Service.Tasks.Domain.Models.Base;

namespace Service.Tasks.Domain.Models;

public class TaskModel : ModelBase
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? ParentId { get; set; }

    public TaskModel? Parent { get; set; }

    public List<TaskModel> Children { get; set; } = [];
}
