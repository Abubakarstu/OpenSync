namespace OpenSync.Core.Interfaces.Clocks;

public interface ISystemClock
{
    DateTime UtcNow { get; }
}
