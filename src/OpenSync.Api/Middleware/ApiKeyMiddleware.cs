namespace OpenSync.Infrastructure.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/v1/sync/tokens") ||
            context.Request.Path.StartsWithSegments("/api/v1/sync/health") ||
            context.Request.Path.StartsWithSegments("/ws"))
        {
            await _next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/api/v1/sync"))
        {
            var hasApiKey = context.Request.Headers.ContainsKey("X-Api-Key");
            var hasBearer = context.Request.Headers["Authorization"].FirstOrDefault()?.StartsWith("Bearer ") == true;

            if (!hasApiKey && !hasBearer)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("{\"success\":false,\"error\":{\"code\":\"UNAUTHORIZED\",\"message\":\"API key or JWT token required.\"}}");
                return;
            }
        }

        await _next(context);
    }
}

public static class ApiKeyMiddlewareExtensions
{
    public static IApplicationBuilder UseApiKey(this IApplicationBuilder builder)
        => builder.UseMiddleware<ApiKeyMiddleware>();
}
