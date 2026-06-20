using OpenSync.Core.Interfaces.Clocks;

namespace OpenSync.Infrastructure.Services;

public class SystemClock : ISystemClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
