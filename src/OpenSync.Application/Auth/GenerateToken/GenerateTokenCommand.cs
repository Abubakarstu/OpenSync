using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Auth.GenerateToken;

public record GenerateTokenCommand(
    string Identity,
    Guid ServiceId,
    Dictionary<string, string[]> Permissions,
    int ExpirationMinutes = 60
) : ICommand<Common.Result<string>>;
