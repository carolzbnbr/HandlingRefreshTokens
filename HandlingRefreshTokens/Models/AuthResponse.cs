using System;
using System.Collections.Generic;

namespace HandlingRefreshTokens.Models
{
    public class AuthResponse
    {
        public AuthResponse()
        {
        }

        public string RefreshToken { get;  set; }

        public string BearerToken { get; set; }

        public bool Success { get; set; }
        
    }
}
