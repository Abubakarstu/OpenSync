namespace OpenSync.Sdk.Retry;

public class ReconnectPolicy
{
    public int MaxRetries { get; set; } = 5;
    public bool IsEnabled { get; set; } = true;
}
