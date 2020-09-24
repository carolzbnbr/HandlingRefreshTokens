using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HandlingRefreshTokens.WebAPIs.DbContexts;
using HandlingRefreshTokens.WebAPIs.DbContexts.Entities;
using HandlingRefreshTokens.WebAPIs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HandlingRefreshTokens.WebAPIs.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(LocalDbContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        private LocalDbContext _context;
        private readonly IConfiguration configuration;

        public AuthResponse AuthByCredentials(AuthByCredentialsRequest request)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == request.Username && x.Password == request.Password);

            if (user == null)
            {
                return null;
            }

            var jwtToken = GenerateBearerToken(user);
            var refreshTokenInfo = GetNewRefreshToken(user.IdUser);

            _context.RefreshTokens.Add(refreshTokenInfo);
            _context.SaveChanges();

            return new AuthResponse { Id = user.IdUser, BearerToken = jwtToken, RefreshToken = refreshTokenInfo.Token, Success = true };
        }

        public AuthResponse AuthByRefreshToken(AuthByRefreshTokenRequest request)
        {
            var previousRefreshToken = _context.RefreshTokens.SingleOrDefault(f => f.Token == request.RefreshToken);

            if (previousRefreshToken == null)
            {
                return null;
            }

            var user = _context.Users.SingleOrDefault(f => f.IdUser == previousRefreshToken.IdUser);

            if (user == null)
            {
                return null;
            }

            var newRefreshToken = GetNewRefreshToken(user.IdUser);
            _context.RefreshTokens.Add(newRefreshToken);
            _context.RefreshTokens.Remove(previousRefreshToken);
            _context.SaveChanges();

            var jwtToken = GenerateBearerToken(user);

            return new AuthResponse { Id = user.IdUser, BearerToken = jwtToken, RefreshToken = newRefreshToken.Token, Success = true };
        }




        private string GenerateBearerToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddSeconds(15),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString())
                }),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var finalToken = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(finalToken);
        }

        private RefreshToken GetNewRefreshToken(int id)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var tokenBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(tokenBytes);
                return new RefreshToken
                {
                    IdUser = id,
                    Token = Convert.ToBase64String(tokenBytes),
                    Expires = DateTime.UtcNow.AddMonths(12),
                };
            }
        }
    }
}
