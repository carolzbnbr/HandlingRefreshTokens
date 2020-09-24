using System;
using System.Threading.Tasks;
using HandlingRefreshTokens.Models;

namespace HandlingRefreshTokens.Services
{
    public interface IRESTService
    {
        string BearerToken { get; }
        string RefreshToken { get; }
       
        Task<AuthResponse> AuthWithCredentialsAsync(string username, string password);

        /// <summary>
        /// Attempts to execute  <paramref name="webApiCallMethod"/> and return its results.
        /// Case the <see cref="BearerToken"/> provided in <paramref name="webApiCallMethod"/> is expired,
        /// attempts to renew it by using the current Refresh-Token, and after that runs the method <paramref name="webApiCallMethod"/> again.
        /// </summary>
        /// <typeparam name="TResponse">The expected response object</typeparam>
        /// <param name="webApiCallMethod">task which contains an httpclient call using the current <see cref="BearerToken"/>.</param>
        /// <returns></returns>
        Task<TResponse> ExecuteWithRetryAsync<TResponse>(Func<Task<TResponse>> method);
    }
}
