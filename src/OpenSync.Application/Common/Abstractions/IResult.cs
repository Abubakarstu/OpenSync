namespace OpenSync.Application.Common.Abstractions;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    string? ErrorCode { get; }
    string? ErrorMessage { get; }
}

public interface IResult<out T> : IResult
{
    T? Data { get; }
}
