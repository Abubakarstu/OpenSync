using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Application.Auth.ValidateToken;

public record ValidateTokenQuery(string Token) : IQuery<Common.Result<TokenPayload>>;
