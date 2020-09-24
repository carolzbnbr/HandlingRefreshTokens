using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HandlingRefreshTokens.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace HandlingRefreshTokens.Services
{
    public class PeopleService : IPeopleService
    {
        private const string URL = "http://10.0.0.199:4444/api/people";
        public IRESTService RESTService => DependencyService.Get<IRESTService>();
        public PeopleService()
        {
        }
       
        public  Task<IEnumerable<PersonInfo>> GetPeopleAsync()
        {
            return RESTService.ExecuteWithRetryAsync(async () =>
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"bearer {RESTService.BearerToken}");
                    
                    var responseMessage = await client.GetAsync(URL);

                    responseMessage.EnsureSuccessStatusCode();

                    var jsonResponse = await responseMessage.Content.ReadAsStringAsync();

                    var response = JsonConvert.DeserializeObject<IEnumerable<PersonInfo>>(jsonResponse);

                    return response;
                }
            });
        }


    }
}
