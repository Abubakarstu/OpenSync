using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OpenSync.Application.Common.Behaviors;
using System.Reflection;

namespace OpenSync.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOpenSyncApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ConcurrencyBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
