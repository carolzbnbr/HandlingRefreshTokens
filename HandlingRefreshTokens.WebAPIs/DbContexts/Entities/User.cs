using System.Text.Json.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HandlingRefreshTokens.WebAPIs.DbContexts.Entities
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
      
        public string Username { get; set; }

        public string Password { get; set; }

    }
}
