using Service.Tasks.Data.Models.Base;

namespace Service.Tasks.Data.Models;

public class TaskEntity : EntityBase
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? ParentId { get; set; }

    public TaskEntity? Parent { get; set; }

    public List<TaskEntity> Children { get; set; } = [];
}
