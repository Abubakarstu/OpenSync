namespace OpenSync.Application.Common;

public class Result : IResult
{
    public bool IsSuccess { get; protected set; }
    public bool IsFailure => !IsSuccess;
    public string? ErrorCode { get; protected set; }
    public string? ErrorMessage { get; protected set; }

    protected Result() { IsSuccess = true; }

    protected Result(string errorCode, string? errorMessage = null)
    {
        IsSuccess = false;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new();
    public static Result<T> Success<T>(T data) => new(data);
    public static Result Failure(string errorCode, string? errorMessage = null) => new(errorCode, errorMessage);
    public static Result<T> Failure<T>(string errorCode, string? errorMessage = null) => new(errorCode, errorMessage);
    public static Result<T> Failure<T>(Exception ex) => new(ex switch
    {
        Core.Exceptions.ConflictException ce => ce.Code,
        Core.Exceptions.NotFoundException ne => ne.Code,
        Core.Exceptions.UnauthorizedException ue => ue.Code,
        Core.Exceptions.PayloadTooLargeException pe => pe.Code,
        Core.Exceptions.QuotaExceededException qe => qe.Code,
        Core.Exceptions.DuplicateException de => de.Code,
        _ => "INTERNAL_ERROR"
    }, ex.Message);
}

public class Result<T> : Result, IResult<T>
{
    public T? Data { get; private set; }

    internal Result(T data)
    {
        IsSuccess = true;
        Data = data;
    }

    internal Result(string errorCode, string? errorMessage = null) : base(errorCode, errorMessage) { }
}
