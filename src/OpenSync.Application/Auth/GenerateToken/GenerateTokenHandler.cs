using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Application.Auth.GenerateToken;

public class GenerateTokenHandler : ICommandHandler<GenerateTokenCommand, Common.Result<string>>
{
    private readonly ITokenService _tokenService;

    public GenerateTokenHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<Common.Result<string>> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var payload = new TokenPayload
        {
            Identity = request.Identity,
            ServiceId = request.ServiceId,
            Permissions = request.Permissions,
            ExpiresAt = DateTime.UtcNow.AddMinutes(request.ExpirationMinutes)
        };

        var token = _tokenService.GenerateToken(payload, "default-secret-key-change-in-production");
        return await Task.FromResult(Common.Result.Success(token));
    }
}
