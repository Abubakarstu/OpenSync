using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Common;
using OpenSync.Application.Common.Models;

namespace OpenSync.Api.Controllers;

[ApiController]
[Route("api/v1/sync")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult ApiResponse(Result result)
    {
        if (result.IsSuccess)
            return Ok(new ApiResponse<object> { Success = true, Data = null });

        return ErrorResult(result);
    }

    protected IActionResult ApiResponse<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            if (result.Data == null)
                return Ok(new ApiResponse<object> { Success = true, Data = null });

            return Ok(new ApiResponse<T> { Success = true, Data = result.Data });
        }

        return ErrorResult(result);
    }

    private IActionResult ErrorResult(Result result)
    {
        return result.ErrorCode switch
        {
            "NOT_FOUND" => NotFound(new ErrorResponse
            {
                Success = false,
                Error = new ErrorDetail { Code = result.ErrorCode!, Message = result.ErrorMessage! }
            }),
            "REVISION_CONFLICT" => Conflict(new ErrorResponse
            {
                Success = false,
                Error = new ErrorDetail { Code = result.ErrorCode!, Message = result.ErrorMessage! }
            }),
            "DUPLICATE" => Conflict(new ErrorResponse
            {
                Success = false,
                Error = new ErrorDetail { Code = result.ErrorCode!, Message = result.ErrorMessage! }
            }),
            "UNAUTHORIZED" => Unauthorized(new ErrorResponse
            {
                Success = false,
                Error = new ErrorDetail { Code = result.ErrorCode!, Message = result.ErrorMessage! }
            }),
            "QUOTA_EXCEEDED" => StatusCode(429, new ErrorResponse
            {
                Success = false,
                Error = new ErrorDetail { Code = result.ErrorCode!, Message = result.ErrorMessage! }
            }),
            "PAYLOAD_TOO_LARGE" => StatusCode(413, new ErrorResponse
            {
                Success = false,
                Error = new ErrorDetail { Code = result.ErrorCode!, Message = result.ErrorMessage! }
            }),
            _ => BadRequest(new ErrorResponse
            {
                Success = false,
                Error = new ErrorDetail { Code = result.ErrorCode!, Message = result.ErrorMessage! }
            })
        };
    }

    protected IActionResult PagedResponse<T>(Result<PagedResult<T>> result)
    {
        if (result.IsSuccess && result.Data != null)
        {
            var response = new PagedResponse<T>
            {
                Success = true,
                Data = result.Data.Items,
                Meta = new PagedResponseMeta
                {
                    Page = result.Data.Page,
                    PageSize = result.Data.PageSize,
                    TotalCount = result.Data.TotalCount,
                    HasNext = result.Data.HasNext
                }
            };
            return Ok(response);
        }

        return ApiResponse(result);
    }
}
