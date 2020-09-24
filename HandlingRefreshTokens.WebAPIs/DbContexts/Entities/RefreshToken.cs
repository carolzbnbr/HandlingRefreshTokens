using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace HandlingRefreshTokens.WebAPIs.DbContexts.Entities
{
    public class RefreshToken
    {
        
        [Key]
        public int Id { get; set; }

        public int IdUser { get; set; }

        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
