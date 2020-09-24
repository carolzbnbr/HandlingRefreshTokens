namespace HandlingRefreshTokens.WebAPIs.Models
{
    public class AuthResponse
    {
        public AuthResponse()
        {

        }
        public int Id { get; set; }
        public string RefreshToken { get; set; }

      
        public string BearerToken { get; set; }

        public bool Success { get; set; }

       
    }
}
