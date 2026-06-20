using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Application.Auth.RefreshToken;

public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, Common.Result<string>>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<Common.Result<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var payload = _tokenService.ValidateToken(request.Token, "default-secret-key-change-in-production");
        if (payload == null)
            return Common.Result.Failure<string>("INVALID_TOKEN", "The provided token is invalid or expired.");

        payload.ExpiresAt = DateTime.UtcNow.AddMinutes(request.NewExpirationMinutes);
        var newToken = _tokenService.GenerateToken(payload, "default-secret-key-change-in-production");
        return await Task.FromResult(Common.Result.Success(newToken));
    }
}
