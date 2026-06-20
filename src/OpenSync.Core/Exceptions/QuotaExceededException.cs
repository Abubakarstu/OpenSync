namespace OpenSync.Core.Exceptions;

public class QuotaExceededException : SyncException
{
    public string QuotaType { get; }
    public int Current { get; }
    public int Max { get; }

    public QuotaExceededException(string quotaType, int current, int max)
        : base("QUOTA_EXCEEDED", $"Quota exceeded for {quotaType}: {current}/{max}.")
    {
        QuotaType = quotaType;
        Current = current;
        Max = max;
    }
}
