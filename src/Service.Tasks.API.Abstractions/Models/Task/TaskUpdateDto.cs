using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Task;

public class TaskUpdateDto
{
    [Required]
    public required string Title { get; set; }

    public string? Description { get; set; }

    public Guid? ParentId { get; set; }
}
