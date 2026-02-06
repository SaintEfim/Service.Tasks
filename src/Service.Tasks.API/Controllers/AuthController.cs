using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Service.Tasks.API.Models.Base;
using Service.Tasks.API.Models.User;
using Service.Tasks.Domain.Models.User;
using Service.Tasks.Domain.Services.User;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Service.Tasks.API.Controllers;

/// <summary>
/// Provides authentication and authorization endpoints for user management.
/// Handles user registration, login, token refresh, and password reset operations.
/// </summary>
[Route("api/v1/Auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserManager _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="userManager">The user manager service for authentication operations.</param>
    public AuthController(
        IMapper mapper,
        IUserManager userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="payload">The user registration data.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An <see cref="AuthenticationDto"/> containing authentication tokens upon successful registration.
    /// Returns HTTP 200 on success, HTTP 400 if the request is invalid.
    /// </returns>
    [HttpPost("register")]
    [OpenApiOperation(nameof(Register))]
    [SwaggerResponse(Status200OK, typeof(AuthenticationDto))]
    [SwaggerResponse(Status400BadRequest, typeof(ErrorDto))]
    public async Task<ActionResult<AuthenticationDto>> Register(
        [FromBody] UserRegisterDto payload,
        CancellationToken cancellationToken = default)
    {
        var userModel = _mapper.Map<UserModel>(payload);
        var result = await _userManager.Register(userModel, null, cancellationToken);
        return Ok(_mapper.Map<AuthenticationDto>(result));
    }

    /// <summary>
    /// Authenticates a user and returns access and refresh tokens.
    /// </summary>
    /// <param name="payload">The user login credentials.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An <see cref="AuthenticationDto"/> containing authentication tokens upon successful login.
    /// Returns HTTP 200 on success, HTTP 400 for invalid request, or HTTP 401 for authentication failure.
    /// </returns>
    [HttpPost("login")]
    [OpenApiOperation(nameof(Login))]
    [SwaggerResponse(Status200OK, typeof(AuthenticationDto))]
    [SwaggerResponse(Status400BadRequest, typeof(ErrorDto))]
    [SwaggerResponse(Status401Unauthorized, typeof(ErrorDto))]
    public async Task<ActionResult<AuthenticationDto>> Login(
        [FromBody] UserLoginDto payload,
        CancellationToken cancellationToken = default)
    {
        var userModel = _mapper.Map<UserModel>(payload);
        var result = await _userManager.Login(userModel, null, cancellationToken);
        return Ok(_mapper.Map<AuthenticationDto>(result));
    }

    /// <summary>
    /// Refreshes an expired access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshTokenDto">The refresh token data transfer object.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A new <see cref="AuthenticationDto"/> with refreshed access and refresh tokens.
    /// Returns HTTP 200 on success, HTTP 400 for invalid request, or HTTP 401 for invalid refresh token.
    /// </returns>
    [HttpPost("refresh")]
    [OpenApiOperation(nameof(Refresh))]
    [SwaggerResponse(Status200OK, typeof(AuthenticationDto))]
    [SwaggerResponse(Status400BadRequest, typeof(ErrorDto))]
    [SwaggerResponse(Status401Unauthorized, typeof(ErrorDto))]
    public async Task<ActionResult<AuthenticationDto>> Refresh(
        RefreshTokenDto refreshTokenDto,
        CancellationToken cancellationToken = default)
    {
        var result = await _userManager.Refresh(refreshTokenDto.RefreshTocken, cancellationToken: cancellationToken);
        return Ok(_mapper.Map<AuthenticationDto>(result));
    }

    /// <summary>
    /// Resets the password for the currently authenticated user.
    /// Requires a valid authentication token.
    /// </summary>
    /// <param name="newPassword">The new password to set for the user.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// HTTP 200 on successful password reset.
    /// Returns HTTP 400 for invalid request or HTTP 401 if the user is not authenticated.
    /// </returns>
    [HttpPost("reset-password")]
    [Authorize]
    [OpenApiOperation(nameof(ResetPassword))]
    [SwaggerResponse(Status200OK, typeof(void))]
    [SwaggerResponse(Status400BadRequest, typeof(ErrorDto))]
    [SwaggerResponse(Status401Unauthorized, typeof(ErrorDto))]
    public async Task<IActionResult> ResetPassword(
        [FromBody] string newPassword,
        CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();
        await _userManager.ResetPassword(userId, newPassword, cancellationToken: cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Extracts the current user's ID from the authentication token claims.
    /// </summary>
    /// <returns>The GUID of the currently authenticated user.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when the user ID claim is missing or invalid in the authentication token.
    /// </exception>
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c =>
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }

        return userId;
    }
}
