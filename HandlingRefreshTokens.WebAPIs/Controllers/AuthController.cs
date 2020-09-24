using HandlingRefreshTokens.WebAPIs.Models;
using HandlingRefreshTokens.WebAPIs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HandlingRefreshTokens.WebAPIs.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult AuthByCredentials([FromBody] AuthByCredentialsRequest request)
        {
            var response = authService.AuthByCredentials(request);

            if (response == null)
            {
                return BadRequest(new { message = "Invalid Username or password" });
            }
            

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public IActionResult AuthByRefreshToken([FromBody] AuthByRefreshTokenRequest request)
        {
           
            var response = authService.AuthByRefreshToken(request);

            if (response == null)
            {
                return Unauthorized(new { message = "The provided token is not valid" });
            }

            return Ok(response);
        }
    }
}
