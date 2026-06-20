using OpenSync.Infrastructure.Services;

namespace OpenSync.Infrastructure.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RateLimitService _rateLimitService;

    public RateLimitingMiddleware(RequestDelegate next, RateLimitService rateLimitService)
    {
        _next = next;
        _rateLimitService = rateLimitService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var limit = 100;

        if (!_rateLimitService.IsAllowed(clientKey, limit, out var remaining, out var resetMs))
        {
            context.Response.StatusCode = 429;
            context.Response.Headers["Retry-After"] = Math.Max(1, (resetMs - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 1000).ToString();
            context.Response.Headers["X-RateLimit-Limit"] = limit.ToString();
            context.Response.Headers["X-RateLimit-Remaining"] = "0";
            context.Response.Headers["X-RateLimit-Reset"] = resetMs.ToString();
            await context.Response.WriteAsync("{\"success\":false,\"error\":{\"code\":\"RATE_LIMITED\",\"message\":\"Too many requests. Please try again later.\"}}");
            return;
        }

        context.Response.Headers["X-RateLimit-Limit"] = limit.ToString();
        context.Response.Headers["X-RateLimit-Remaining"] = remaining.ToString();
        context.Response.Headers["X-RateLimit-Reset"] = resetMs.ToString();

        await _next(context);
    }
}

public static class RateLimitingMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        => builder.UseMiddleware<RateLimitingMiddleware>();
}
