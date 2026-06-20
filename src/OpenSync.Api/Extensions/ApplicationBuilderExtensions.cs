using OpenSync.Infrastructure.Auth;

namespace OpenSync.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder builder)
        => builder.UseMiddleware<TokenValidationMiddleware>();
}
