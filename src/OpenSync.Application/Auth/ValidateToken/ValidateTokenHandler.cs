using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Application.Auth.ValidateToken;

public class ValidateTokenHandler : IQueryHandler<ValidateTokenQuery, Common.Result<TokenPayload>>
{
    private readonly ITokenService _tokenService;

    public ValidateTokenHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<Common.Result<TokenPayload>> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
    {
        var payload = _tokenService.ValidateToken(request.Token, "default-secret-key-change-in-production");
        if (payload == null)
            return Common.Result.Failure<TokenPayload>("INVALID_TOKEN", "The provided token is invalid or expired.");

        return await Task.FromResult(Common.Result.Success(payload));
    }
}
