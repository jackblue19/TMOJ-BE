namespace WebAPI.Middlewares;

public sealed class RequestLogScopeMiddleware
{
    private readonly RequestDelegate _next;
    public RequestLogScopeMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx , ILogger<RequestLogScopeMiddleware> logger)
    {
        using ( logger.BeginScope(new Dictionary<string , object?>
        {
            ["TraceId"] = ctx.TraceIdentifier
        }) )
        {
            await _next(ctx);
        }
    }
}

