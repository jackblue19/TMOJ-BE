using Application.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Extensions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetails;
    private readonly IHostEnvironment _env;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(IProblemDetailsService problemDetails ,
                                    IHostEnvironment env ,
                                    ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetails = problemDetails;
        _env = env;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext ,
                                                Exception exception ,
                                                CancellationToken ct)
    {
        var (status, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflict"),

            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error")
        };

        _logger.LogError(
                    exception ,
                    "Unhandled exception. Status={StatusCode} TraceId={TraceId}" ,
                    status ,
                    httpContext.TraceIdentifier);

        var includeDetail = _env.IsDevelopment();

        httpContext.Response.StatusCode = status;

        return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext ,
            ProblemDetails = new ProblemDetails
            {
                Status = status ,
                Title = title ,
                Detail = includeDetail ? exception.Message : null
            }
        });
    }
}
