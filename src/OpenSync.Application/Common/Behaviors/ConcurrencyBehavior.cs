using MediatR;
using Microsoft.Extensions.Logging;

namespace OpenSync.Application.Common.Behaviors;

public class ConcurrencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<ConcurrencyBehavior<TRequest, TResponse>> _logger;

    public ConcurrencyBehavior(ILogger<ConcurrencyBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        const int maxRetries = 3;
        int retryCount = 0;

        while (true)
        {
            try
            {
                return await next();
            }
            catch (Core.Exceptions.ConflictException ex) when (retryCount < maxRetries)
            {
                retryCount++;
                _logger.LogWarning("Concurrency conflict on {ObjectType} {ObjectId}, retry {RetryCount}/{MaxRetries}",
                    ex.ObjectType, ex.ObjectId, retryCount, maxRetries);
                await Task.Delay(50 * retryCount, cancellationToken);
            }
        }
    }
}
