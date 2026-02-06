using System.ComponentModel.DataAnnotations;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.API.Models.User;

public class UserRegisterDto
{
    [Required]
    public required string UserName { get; set; }

    [Required]
    public required RoleEnum Role { get; set; }

    [Required]
    public required string Password { get; set; }
}
