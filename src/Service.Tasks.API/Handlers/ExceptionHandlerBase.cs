using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Service.Tasks.API.Models.Base;

namespace Service.Tasks.API.Handlers;

internal sealed class ExceptionHandlerBase : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorDto = new ErrorDto
        {
            Title = "Internal Server Error",
            Description = exception.Message,
            StatusCode = StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(errorDto);
        await httpContext.Response.WriteAsync(json, cancellationToken);

        return true;
    }
}
