using System.ComponentModel.DataAnnotations;
using Service.Tasks.API.Models.Base;

namespace Service.Tasks.API.Models.Task;

public class TaskDto : DtoBase
{
    [Required]
    public required string Title { get; set; }

    public string? Description { get; set; }

    public TaskDto? Parent { get; set; }
}
