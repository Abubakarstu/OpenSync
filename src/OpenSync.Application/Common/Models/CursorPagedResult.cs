namespace OpenSync.Application.Common.Models;

public class CursorPagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public string? NextCursor { get; }
    public bool HasNext { get; }

    public CursorPagedResult(IReadOnlyList<T> items, string? nextCursor = null, bool hasNext = false)
    {
        Items = items;
        NextCursor = nextCursor;
        HasNext = hasNext;
    }
}
