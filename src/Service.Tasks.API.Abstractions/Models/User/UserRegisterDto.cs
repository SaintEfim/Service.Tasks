using System.ComponentModel.DataAnnotations;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.API.Models.User;

/// <summary>
/// Represents user registration data.
/// </summary>
public class UserRegisterDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required]
    public required string UserName { get; set; }

    /// <summary>
    /// Gets or sets the user role.
    /// </summary>
    [Required]
    public required RoleEnum Role { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [Required]
    public required string Password { get; set; }
}
