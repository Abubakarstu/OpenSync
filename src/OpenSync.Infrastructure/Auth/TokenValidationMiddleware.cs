using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenSync.Core.Interfaces.Services;
using System.Security.Claims;

namespace OpenSync.Infrastructure.Auth;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidationMiddleware> _logger;

    public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var token = authHeader.FirstOrDefault()?.Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                var tokenService = context.RequestServices.GetRequiredService<ITokenService>();
                var payload = tokenService.ValidateToken(token, "default-secret-key-change-in-production");
                if (payload != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, payload.Identity),
                        new("service_id", payload.ServiceId.ToString()),
                        new("identity", payload.Identity)
                    };

                    var identity = new ClaimsIdentity(claims, "jwt");
                    context.User = new ClaimsPrincipal(identity);
                }
            }
        }

        await _next(context);
    }
}
