using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace Service.Tasks.UserHelpers.Helpers;

public class BCryptPasswordHasher : IBCryptPasswordHasher
{
    private readonly ILogger<BCryptPasswordHasher> _logger;

    public BCryptPasswordHasher(
        ILogger<BCryptPasswordHasher> logger)
    {
        _logger = logger;
    }

    public string Hash(
        string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(data, 12);
        }

        _logger.LogError("Parameter must not be null or empty.");
        throw new NullReferenceException("Data must not be null or empty.");
    }

    public bool Verify(
        string data,
        string hashedData)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new NullReferenceException("Data must not be null or empty.");
        }

        if (string.IsNullOrEmpty(hashedData))
        {
            throw new BcryptAuthenticationException("Hashed data must not be null or empty.");
        }

        try
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(data, hashedData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying hashed data.");
            throw;
        }
    }
}
