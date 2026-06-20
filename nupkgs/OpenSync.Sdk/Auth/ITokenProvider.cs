namespace OpenSync.Sdk.Auth;

public interface ITokenProvider
{
    Task<string> GetTokenAsync();
}
