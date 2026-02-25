using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

namespace WebAPI.Extensions;

public static class RateLimitingExtensions
{
    public const string HeavyPolicyName = "HeavyEndpoints";

    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // Trả ProblemDetails khi bị limit
            options.OnRejected = async (context , ct) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status429TooManyRequests ,
                    Title = "Too Many Requests" ,
                    Detail = "Rate limit exceeded. Please retry later."
                };

                problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                // Retry-After nếu limiter có metadata
                if ( context.Lease.TryGetMetadata(MetadataName.RetryAfter , out var retryAfter) )
                {
                    context.HttpContext.Response.Headers.RetryAfter = ((int) retryAfter.TotalSeconds).ToString();
                    problem.Extensions["retryAfterSeconds"] = (int) retryAfter.TotalSeconds;
                }

                await context.HttpContext.Response.WriteAsJsonAsync(problem , cancellationToken: ct);
            };

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // GLOBAL limiter: áp cho tất cả endpoints (trừ khi bạn map khác)
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext , string>(httpContext =>
            {
                var key = GetClientKey(httpContext);
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: key ,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 200 ,
                        Window = TimeSpan.FromMinutes(1) ,
                        QueueLimit = 0 ,
                        AutoReplenishment = true
                    });
            });

            // Policy cho endpoint “nặng”
            options.AddPolicy(HeavyPolicyName , httpContext =>
            {
                var key = GetClientKey(httpContext);
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: key ,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 30 ,
                        Window = TimeSpan.FromMinutes(1) ,
                        QueueLimit = 0 ,
                        AutoReplenishment = true
                    });
            });
        });

        return services;
    }

    private static string GetClientKey(HttpContext ctx)
    {
        // Nếu có auth user thì key theo user (ổn định hơn IP)
        var userId = ctx.User?.Identity?.IsAuthenticated == true
            ? ctx.User.Identity!.Name
            : null;

        if ( !string.IsNullOrWhiteSpace(userId) )
            return $"user:{userId}";

        // Fallback theo IP
        var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return $"ip:{ip}";
    }
}
