namespace OpenSync.Core.ValueObjects;

public class ObjectId : IEquatable<ObjectId>
{
    public string Prefix { get; }
    public Guid Id { get; }
    public string Full => $"{Prefix}_{Id:N}";

    public ObjectId(string prefix, Guid id)
    {
        Prefix = prefix.ToLowerInvariant();
        Id = id;
    }

    public static ObjectId New(string prefix) => new(prefix, Guid.NewGuid());
    public static ObjectId Parse(string full)
    {
        var parts = full.Split('_', 2);
        if (parts.Length != 2 || !Guid.TryParse(parts[1], out var id))
            throw new FormatException($"Invalid ObjectId format: {full}");
        return new ObjectId(parts[0], id);
    }

    public bool Equals(ObjectId? other)
    {
        if (other is null) return false;
        return Prefix == other.Prefix && Id == other.Id;
    }

    public override bool Equals(object? obj) => obj is ObjectId other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Prefix, Id);
    public override string ToString() => Full;
}
