using Microsoft.IdentityModel.Tokens;
using OpenSync.Core.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace OpenSync.Infrastructure.Auth;

public class JwtTokenService : ITokenService
{
    public string GenerateToken(TokenPayload payload, string secretKey)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, payload.Identity),
            new("service_id", payload.ServiceId.ToString()),
            new("permissions", JsonSerializer.Serialize(payload.Permissions)),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: "opensync",
            audience: "opensync-clients",
            claims: claims,
            expires: payload.ExpiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokenPayload? ValidateToken(string token, string secretKey)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = "opensync",
                ValidateAudience = true,
                ValidAudience = "opensync-clients",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var identity = result.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty;
            var serviceIdStr = result.FindFirst("service_id")?.Value ?? Guid.Empty.ToString();
            var permissionsStr = result.FindFirst("permissions")?.Value ?? "{}";

            return new TokenPayload
            {
                Identity = identity,
                ServiceId = Guid.Parse(serviceIdStr),
                Permissions = JsonSerializer.Deserialize<Dictionary<string, string[]>>(permissionsStr) ?? new(),
                ExpiresAt = (validatedToken as JwtSecurityToken)?.ValidTo ?? DateTime.UtcNow
            };
        }
        catch
        {
            return null;
        }
    }
}
