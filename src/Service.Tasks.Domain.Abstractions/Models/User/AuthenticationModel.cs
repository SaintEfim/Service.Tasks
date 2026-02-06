namespace Service.Tasks.Domain.Models.User;

public class AuthenticationModel
{
    public required string TokenType { get; set; }

    public required string AccessToken { get; set; }

    public required int ExpiresIn { get; set; }

    public required string RefreshToken { get; set; }
}
