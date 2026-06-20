namespace OpenSync.Core.ValueObjects;

public readonly record struct TimeToLive(TimeSpan Duration)
{
    public DateTime ExpiresAt => DateTime.UtcNow.Add(Duration);
    public bool IsExpired(DateTime? now = null) => (now ?? DateTime.UtcNow) >= ExpiresAt;

    public static TimeToLive FromMinutes(int minutes) => new(TimeSpan.FromMinutes(minutes));
    public static TimeToLive FromHours(int hours) => new(TimeSpan.FromHours(hours));
    public static TimeToLive FromDays(int days) => new(TimeSpan.FromDays(days));

    public static TimeToLive Default => new(TimeSpan.FromHours(24));
}
