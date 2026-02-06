using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.User;

public class AuthenticationDto
{
    [Required]
    public required string TokenType { get; set; }

    [Required]
    public required string AccessToken { get; set; }

    [Required]
    public required int ExpiresIn { get; set; }

    [Required]
    public required string RefreshToken { get; set; }
}
