using OpenSync.Application.Common.Models;

namespace OpenSync.Api.Models.Responses;

public class ErrorResponse
{
    public bool Success { get; set; }
    public ErrorDetail? Error { get; set; }
}
