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

[Route("api/v1/Auth")]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserManager _userManager;

    public AuthController(
        IMapper mapper,
        IUserManager userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

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

    [HttpPost("refresh")]
    [OpenApiOperation(nameof(Refresh))]
    [SwaggerResponse(Status200OK, typeof(AuthenticationDto))]
    [SwaggerResponse(Status400BadRequest, typeof(ErrorDto))]
    [SwaggerResponse(Status401Unauthorized, typeof(ErrorDto))]
    public async Task<ActionResult<AuthenticationDto>> Refresh(
        [FromBody] string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var result = await _userManager.Refresh(refreshToken, null, cancellationToken);
        return Ok(_mapper.Map<AuthenticationDto>(result));
    }

    [Authorize]
    [HttpPost("reset-password")]
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

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }

        return userId;
    }
}
