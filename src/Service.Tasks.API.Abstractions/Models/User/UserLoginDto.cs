using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.User;

public class UserLoginDto
{
    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }
}
