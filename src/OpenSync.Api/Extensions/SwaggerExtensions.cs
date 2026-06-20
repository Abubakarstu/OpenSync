using Microsoft.OpenApi.Models;

namespace OpenSync.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddOpenSyncSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "OpenSync API",
                Version = "v1",
                Description = "General-purpose real-time data synchronization API"
            });

            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key authentication via X-Api-Key header",
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKey"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Bearer token authentication",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                    },
                    Array.Empty<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
