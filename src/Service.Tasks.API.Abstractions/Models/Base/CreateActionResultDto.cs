using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Base;

public class CreateActionResultDto : IDto
{
    [Required]
    public Guid Id { get; set; }
}
