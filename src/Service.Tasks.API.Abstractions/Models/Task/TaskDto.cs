using System.ComponentModel.DataAnnotations;
using Service.Tasks.API.Models.Base;

namespace Service.Tasks.API.Models.Task;

/// <summary>
/// Represents a task data transfer object.
/// </summary>
public class TaskDto : DtoBase
{
    /// <summary>
    /// Gets or sets the task title.
    /// </summary>
    [Required]
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the task description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the parent task.
    /// </summary>
    public TaskDto? Parent { get; set; }
}
