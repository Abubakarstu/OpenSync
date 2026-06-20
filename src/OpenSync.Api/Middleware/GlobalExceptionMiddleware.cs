using System.Net;
using System.Text.Json;
using FluentValidation;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Common.Models;

namespace OpenSync.Infrastructure.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            Core.Exceptions.NotFoundException => HttpStatusCode.NotFound,
            Core.Exceptions.ConflictException => HttpStatusCode.Conflict,
            Core.Exceptions.UnauthorizedException => HttpStatusCode.Unauthorized,
            Core.Exceptions.PayloadTooLargeException => HttpStatusCode.RequestEntityTooLarge,
            Core.Exceptions.QuotaExceededException => HttpStatusCode.TooManyRequests,
            Core.Exceptions.DuplicateException => HttpStatusCode.Conflict,
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            Success = false,
            Error = new ErrorDetail
            {
                Code = exception switch
                {
                    Core.Exceptions.SyncException se => se.Code,
                    ValidationException => "VALIDATION_ERROR",
                    _ => "INTERNAL_ERROR"
                },
                Message = exception.Message
            }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }));
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
        => builder.UseMiddleware<GlobalExceptionMiddleware>();
}
