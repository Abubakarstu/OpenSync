namespace OpenSync.Api.Models.Responses;

public class PagedResponse<T>
{
    public bool Success { get; set; }
    public IReadOnlyList<T>? Data { get; set; }
    public PagedResponseMeta? Meta { get; set; }
}

public class PagedResponseMeta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasNext { get; set; }
    public string? NextCursor { get; set; }
}
