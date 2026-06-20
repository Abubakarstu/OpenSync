namespace OpenSync.Core.ValueObjects;

public class Permission : IEquatable<Permission>
{
    public string ObjectType { get; }
    public IReadOnlyList<string> Actions { get; }

    public Permission(string objectType, IEnumerable<string> actions)
    {
        ObjectType = objectType.ToLowerInvariant();
        Actions = actions.Select(a => a.ToLowerInvariant()).ToList().AsReadOnly();
    }

    public bool Allows(string action) => Actions.Contains("*") || Actions.Contains(action.ToLowerInvariant());
    public bool AllowsAny(params string[] actions) => actions.Any(a => Allows(a));
    public bool AllowsAll(params string[] actions) => actions.All(a => Allows(a));

    public bool Equals(Permission? other)
    {
        if (other is null) return false;
        return ObjectType == other.ObjectType && Actions.SequenceEqual(other.Actions);
    }

    public override bool Equals(object? obj) => obj is Permission other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(ObjectType, string.Join(",", Actions));
}
