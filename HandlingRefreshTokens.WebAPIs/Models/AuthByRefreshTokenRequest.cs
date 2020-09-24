using System.ComponentModel.DataAnnotations;
namespace HandlingRefreshTokens.WebAPIs.Models
{
    public class AuthByRefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }

    }
}
