using Kf.Service.Warehouse.Domain.Models.Base;

namespace Service.Task.Domain.Models;

public class TaskModel : ModelBase
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public TaskModel? Parent { get; set; }

    public List<TaskModel> Children { get; set; } = [];
}
