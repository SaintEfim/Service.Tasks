using System.Security.Claims;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.UserJWTToken.Helpers;

public interface IJwtTokenGenerator
{
    Task<string> GenerateToken(
        string userId,
        RoleEnum role,
        string secretKey,
        TimeSpan expiresIn,
        CancellationToken cancellationToken = default);

    Task<ClaimsPrincipal> DecodeToken(
        string token,
        string secretKey,
        CancellationToken cancellationToken = default);
}
