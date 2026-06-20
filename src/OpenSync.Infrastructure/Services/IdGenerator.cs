using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Infrastructure.Services;

public class IdGenerator : IIdGenerator
{
    public string NewId(string prefix)
    {
        return $"{prefix}_{Guid.NewGuid():N}";
    }
}
