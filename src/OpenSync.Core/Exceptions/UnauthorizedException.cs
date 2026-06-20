namespace OpenSync.Core.Exceptions;

public class UnauthorizedException : SyncException
{
    public string? Identity { get; }
    public string? RequiredPermission { get; }

    public UnauthorizedException(string? identity = null, string? requiredPermission = null)
        : base("UNAUTHORIZED", $"Unauthorized access{ (identity != null ? $" for identity '{identity}'" : "") }.{ (requiredPermission != null ? $" Requires '{requiredPermission}' permission." : "") }")
    {
        Identity = identity;
        RequiredPermission = requiredPermission;
    }
}
