using HandlingRefreshTokens.WebAPIs.Models;

namespace HandlingRefreshTokens.WebAPIs.Services
{
    public interface IAuthService
    {
        AuthResponse AuthByCredentials(AuthByCredentialsRequest request);
        AuthResponse AuthByRefreshToken(AuthByRefreshTokenRequest request);
    }
}