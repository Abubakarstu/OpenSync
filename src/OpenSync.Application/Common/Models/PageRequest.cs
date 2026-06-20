namespace OpenSync.Application.Common.Models;

public class PageRequest
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 50;

    public string? Cursor { get; set; }
    public int? Limit { get; set; }

    public void EnsureValid()
    {
        if (Page < 0) Page = 0;
        if (PageSize < 1) PageSize = 50;
        if (PageSize > 1000) PageSize = 1000;
    }
}
