using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace Service.Tasks.UserHelpers.Helpers;

internal sealed class BCryptPasswordHasher : IBCryptPasswordHasher
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
        if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(hashedData)) return false;

        if (hashedData.Length != 60 || !(hashedData.StartsWith("$2a$") || hashedData.StartsWith("$2b$") ||
                                         hashedData.StartsWith("$2y$")))
        {
            _logger.LogWarning("Invalid BCrypt hash format detected.");
            return false;
        }

        try
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(data, hashedData);
        }
        catch (SaltParseException)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(data, hashedData);
            }
            catch
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
}
