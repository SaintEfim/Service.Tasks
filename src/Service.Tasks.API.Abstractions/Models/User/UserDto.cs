using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.User;

public class UserDto
{
    [Required]
    public required string UserName { get; set; }
}
