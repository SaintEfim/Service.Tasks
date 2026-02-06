using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.User;

/// <summary>
/// Represents user data.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required]
    public required string UserName { get; set; }
}
