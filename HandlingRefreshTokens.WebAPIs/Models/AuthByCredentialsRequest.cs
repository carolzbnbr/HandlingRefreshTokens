using System.ComponentModel.DataAnnotations;
namespace HandlingRefreshTokens.WebAPIs.Models
{
    public class AuthByCredentialsRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }


        
    }
}
