using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Auth.RefreshToken;

public record RefreshTokenCommand(string Token, int NewExpirationMinutes = 60) : ICommand<Common.Result<string>>;
