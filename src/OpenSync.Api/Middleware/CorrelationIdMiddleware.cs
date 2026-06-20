using Microsoft.Extensions.Logging;

namespace OpenSync.Infrastructure.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
        if (string.IsNullOrEmpty(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers["X-Correlation-ID"] = correlationId;

        using (var scope = context.RequestServices.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CorrelationIdMiddleware>>();
            logger.LogInformation("Request {Method} {Path} with correlation {CorrelationId}",
                context.Request.Method, context.Request.Path, correlationId);
        }

        await _next(context);
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
        => builder.UseMiddleware<CorrelationIdMiddleware>();
}
