using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Base;

public class DtoBase : IDto
{
    [Required]
    public Guid Id { get; set; }
}
