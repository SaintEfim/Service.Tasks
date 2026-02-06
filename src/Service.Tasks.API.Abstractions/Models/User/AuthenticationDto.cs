using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.User;

/// <summary>
/// Represents authentication response data.
/// </summary>
public class AuthenticationDto
{
    /// <summary>
    /// Gets or sets the token type.
    /// </summary>
    [Required]
    public required string TokenType { get; set; }

    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    [Required]
    public required string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the token expiration time in seconds.
    /// </summary>
    [Required]
    public required int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    [Required]
    public required string RefreshToken { get; set; }
}
