using Microsoft.AspNetCore.Mvc;

namespace OpenSync.Api.Models.Requests;

public class ApiPageRequest
{
    [FromQuery]
    public int Page { get; set; } = 0;

    [FromQuery]
    public int PageSize { get; set; } = 50;

    public Application.Common.Models.PageRequest ToApplication() => new()
    {
        Page = Page,
        PageSize = PageSize
    };
}
