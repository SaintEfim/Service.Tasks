using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.User;

public class RefreshTokenDto
{
    [Required]
    public required string RefreshTocken { get; set; }
}
