using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Requests;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Auth.GenerateToken;
using OpenSync.Application.Auth.RefreshToken;
using OpenSync.Application.Auth.ValidateToken;
using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Api.Controllers.V1;

public class AuthController : BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [AllowAnonymous]
    [HttpPost("tokens")]
    public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request)
    {
        var permissions = request.Permissions ?? new Dictionary<string, string[]>();
        var command = new GenerateTokenCommand(request.Identity, request.ServiceId, permissions, request.ExpirationMinutes);
        var result = await _mediator.Send(command);

        if (result.IsSuccess && result.Data != null)
        {
            var tokenService = HttpContext.RequestServices.GetRequiredService<ITokenService>();
            var payload = tokenService.ValidateToken(result.Data, "default-secret-key-change-in-production");
            return Ok(new ApiResponse<TokenResponse>
            {
                Success = true,
                Data = new TokenResponse
                {
                    Token = result.Data,
                    ExpiresAt = payload?.ExpiresAt ?? DateTime.UtcNow.AddMinutes(request.ExpirationMinutes)
                }
            });
        }

        return ApiResponse(result);
    }

    [AllowAnonymous]
    [HttpPost("tokens/validate")]
    public async Task<IActionResult> ValidateToken([FromBody] string token)
    {
        var result = await _mediator.Send(new ValidateTokenQuery(token));
        return ApiResponse(result);
    }

    [AllowAnonymous]
    [HttpPost("tokens/refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string token)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(token));
        if (result.IsSuccess && result.Data != null)
        {
            var tokenService = HttpContext.RequestServices.GetRequiredService<ITokenService>();
            var payload = tokenService.ValidateToken(result.Data, "default-secret-key-change-in-production");
            return Ok(new ApiResponse<TokenResponse>
            {
                Success = true,
                Data = new TokenResponse
                {
                    Token = result.Data,
                    ExpiresAt = payload?.ExpiresAt ?? DateTime.UtcNow.AddHours(1)
                }
            });
        }

        return ApiResponse(result);
    }
}
