namespace OpenSync.Sdk.Retry;

public class ExponentialBackoff
{
    private readonly int _baseDelayMs;

    public ExponentialBackoff(int baseDelayMs = 1000)
    {
        _baseDelayMs = baseDelayMs;
    }

    public TimeSpan GetDelay(int retryAttempt)
        => TimeSpan.FromMilliseconds(Math.Min(_baseDelayMs * Math.Pow(2, retryAttempt), 30000));
}
