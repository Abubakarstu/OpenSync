using OpenSync.Core.Interfaces.Services;
using System.Text;
using System.Text.Json;

namespace OpenSync.Infrastructure.Services;

public class JsonValidator : IJsonValidator
{
    private static readonly JsonDocumentOptions _options = new()
    {
        AllowTrailingCommas = false,
        MaxDepth = 32
    };

    public bool IsValid(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json, _options);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public long GetSize(string json) => Encoding.UTF8.GetByteCount(json);

    public (bool IsValid, string? ErrorMessage) Validate(string json, long? maxSizeBytes = null)
    {
        if (string.IsNullOrWhiteSpace(json))
            return (false, "JSON payload cannot be empty.");

        if (maxSizeBytes.HasValue && json.Length > maxSizeBytes.Value)
            return (false, $"JSON payload size ({json.Length} bytes) exceeds maximum ({maxSizeBytes.Value} bytes).");

        try
        {
            using var doc = JsonDocument.Parse(json, _options);
            return (true, null);
        }
        catch (JsonException ex)
        {
            return (false, $"Invalid JSON: {ex.Message}");
        }
    }
}
