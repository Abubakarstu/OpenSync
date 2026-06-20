namespace OpenSync.Application.Common.Extensions;

public static class ResultExtensions
{
    public static T? GetValueOrDefault<T>(this Result<T> result, T? defaultValue = default) =>
        result.IsSuccess ? result.Data : defaultValue;

    public static Result<T> ToResult<T>(this T value) => Result.Success(value);

    public static Result<T> Ensure<T>(this T value, Func<T, bool> predicate, string errorCode, string errorMessage)
    {
        if (!predicate(value))
            return Result.Failure<T>(errorCode, errorMessage);
        return Result.Success(value);
    }
}
