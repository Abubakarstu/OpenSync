namespace OpenSync.Core.ValueObjects;

public readonly record struct Revision(long Value)
{
    public static Revision Initial => new(0);
    public Revision Next() => new(Value + 1);
    public override string ToString() => Value.ToString();
}
