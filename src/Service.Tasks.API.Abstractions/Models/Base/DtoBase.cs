using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Base;

public class DtoBase : IDto
{
    [Required]
    public required Guid Id { get; set; }
}
