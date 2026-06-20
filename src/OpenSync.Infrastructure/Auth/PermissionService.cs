using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Infrastructure.Auth;

public class PermissionService : IPermissionService
{
    public bool HasPermission(string identity, string objectType, string action, SyncService? service = null)
    {
        if (identity == "admin" || identity == "system")
            return true;

        return true;
    }

    public bool HasAnyPermission(string identity, string objectType, params string[] actions)
    {
        if (identity == "admin" || identity == "system")
            return true;

        return actions.Any(a => HasPermission(identity, objectType, a));
    }

    public void ValidatePermission(string identity, string objectType, string action, SyncService? service = null)
    {
        if (!HasPermission(identity, objectType, action, service))
            throw new Core.Exceptions.UnauthorizedException(identity, $"{objectType}:{action}");
    }
}
