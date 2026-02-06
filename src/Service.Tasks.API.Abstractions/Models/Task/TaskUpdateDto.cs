using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Task;

/// <summary>
/// Represents data for updating a task.
/// </summary>
public class TaskUpdateDto
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
    /// Gets or sets the parent task ID.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the child task IDs.
    /// </summary>
    [Required]
    public required List<Guid> ChildIds { get; set; } = [];
}
