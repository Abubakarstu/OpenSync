namespace OpenSync.Core.Interfaces.Services;

public interface IJsonValidator
{
    bool IsValid(string json);
    long GetSize(string json);
    (bool IsValid, string? ErrorMessage) Validate(string json, long? maxSizeBytes = null);
}
