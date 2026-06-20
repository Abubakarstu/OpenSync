namespace OpenSync.Api.Models.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public object? Meta { get; set; }
}
