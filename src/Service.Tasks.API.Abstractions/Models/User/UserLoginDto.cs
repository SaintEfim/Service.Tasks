using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.User;

/// <summary>
/// Represents user login data.
/// </summary>
public class UserLoginDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required]
    public required string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [Required]
    public required string Password { get; set; }
}
