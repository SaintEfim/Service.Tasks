using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Base;

public class ErrorDto
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public required int StatusCode { get; set; }
}
