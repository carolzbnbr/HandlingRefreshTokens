using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HandlingRefreshTokens.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace HandlingRefreshTokens.Services
{

    /// <summary>
    /// Responsible for handle Restful calls based on JWT and Refresh-tokens.
    /// </summary>
    public class RESTService : IRESTService
    {
        public RESTService()
        {
        }

        private HttpClient client = new HttpClient();

        private static int _refreshTokenEntered = 0;
        public string BearerToken => Preferences.Get("BearerToken", string.Empty);
        public string RefreshToken => Preferences.Get("RefreshToken", string.Empty);
        private const string REFRESH_TOKEN_URL = "http://10.0.0.199:4444/auth/refreshtoken";
        private const string AUTH_URL = "http://10.0.0.199:4444/auth";

        //private const string REFRESH_TOKEN_URL = "https://localhost:47629/auth/refreshtoken";
        //private const string AUTH_URL = "https://localhost:47629/auth";



        /// <summary>
        /// Attempts to execute  <paramref name="webApiCallMethod"/> and return its results.
        /// Case the <see cref="BearerToken"/> provided in <paramref name="webApiCallMethod"/> is expired,
        /// attempts to renew it by using the current Refresh-Token, and after that runs the method <paramref name="webApiCallMethod"/> again.
        /// </summary>
        /// <typeparam name="TResponse">The expected response object</typeparam>
        /// <param name="webApiCallMethod">task which contains an httpclient call using the current <see cref="BearerToken"/>.</param>
        /// <returns></returns>
        public async Task<TResponse> ExecuteWithRetryAsync<TResponse>(Func<Task<TResponse>> webApiCallMethod)
        {
            var tryForceRefreshToken = false;
            var attemptsCounter = 1;
            while (true)
            {
                if (tryForceRefreshToken)
                {
                    var success = await TryAuthWithRefreshTokenAsync();
                }
                try
                {
                    attemptsCounter++;

                    var response = await webApiCallMethod.Invoke();
                    return response;
                }
                catch (HttpRequestException)
                {
                    if (attemptsCounter > 2)
                    {
                        throw;
                    }
                    tryForceRefreshToken = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public async Task<AuthResponse> AuthWithCredentialsAsync(string username, string password)
        {
            dynamic jsonObject = new JObject();
            jsonObject.Username = username;
            jsonObject.Password = password;
          
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(AUTH_URL, content);

            if (responseMessage.IsSuccessStatusCode)
            {
                var stringResponse = await responseMessage.Content.ReadAsStringAsync();
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(stringResponse);

                Preferences.Set("BearerToken", authResponse.BearerToken);
                Preferences.Set("RefreshToken", authResponse.RefreshToken);

                return authResponse;
            }
            else
            {
                return new AuthResponse();
            }
        }

      

      

        private async Task<bool> TryAuthWithRefreshTokenAsync()
        {
            try
            {
                //Tenta executar o refreshtoken apenas da primeira thread que solicitou...
                //Para as demais threads, faz com que elas aguardem pela renovacao do token.
                if (Interlocked.CompareExchange(ref _refreshTokenEntered, 1, 0) == 0)
                {

                    Console.WriteLine("Refresh Token Renewing...");
                    //tenta renovar
                    var authResponse = await AuthWithRefreshTokenAsync();
                  
                    Interlocked.Exchange(ref _refreshTokenEntered, 0);
                    Console.WriteLine("Refresh Token Renewed");
                    return authResponse.Success;
                }
                else
                {
                    Console.WriteLine("Refresh Token Renewal is Waiting...");

                    while (_refreshTokenEntered == 1)
                    {
                        await Task.Delay(100);
                    }
                    //Faz as outras threads aguardarem até que o token seja renovado no bloco anterior
                    Console.WriteLine("Refresh Token Renewal done!");
                    return true;
                }
            }
            
            catch (Exception)
            {
                Interlocked.Exchange(ref _refreshTokenEntered, 0);
                throw;
            }

        }

        private async Task<AuthResponse> AuthWithRefreshTokenAsync()
        {
            dynamic jsonObject = new JObject();
            jsonObject.RefreshToken = Preferences.Get("RefreshToken", string.Empty);

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(REFRESH_TOKEN_URL, content);

            if (responseMessage.IsSuccessStatusCode)
            {
                var stringResponse = await responseMessage.Content.ReadAsStringAsync();
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(stringResponse);

                Preferences.Set("BearerToken", authResponse.BearerToken);
                Preferences.Set("RefreshToken", authResponse.RefreshToken);

                return authResponse;
            }
            else
            {
                return new AuthResponse();
            }

        }

    }

}
