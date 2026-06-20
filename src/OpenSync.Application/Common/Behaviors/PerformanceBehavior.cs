using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace OpenSync.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private const int _thresholdMs = 500;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > _thresholdMs)
        {
            _logger.LogWarning("Long running request: {RequestName} took {ElapsedMs}ms (threshold: {ThresholdMs}ms)",
                typeof(TRequest).Name, stopwatch.ElapsedMilliseconds, _thresholdMs);
        }

        return response;
    }
}
