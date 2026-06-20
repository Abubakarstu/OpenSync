using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Services;

public interface IPermissionService
{
    bool HasPermission(string identity, string objectType, string action, SyncService? service = null);
    bool HasAnyPermission(string identity, string objectType, params string[] actions);
    void ValidatePermission(string identity, string objectType, string action, SyncService? service = null);
}
