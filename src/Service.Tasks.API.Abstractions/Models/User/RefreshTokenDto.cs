using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.User;

/// <summary>
/// Represents refresh token request data.
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    [Required]
    public required string RefreshTocken { get; set; }
}
