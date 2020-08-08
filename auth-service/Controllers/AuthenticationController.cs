using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace auth_service.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult PostLogin(LoginRequest loginRequest)
        {
            // check the username and password
            
            // if valid set the claim in the cookie
            
            // if not valid send a 401
            
        }

        [HttpGet("token")]
        public IEnumerable<LoginResponse> GetToken()
        {
            // check the claim in the cookie
            
            // if authenticated respond with a jwt
            
            // if not authenticated respond with 401
        }
    }

    public class LoginResponse
    {
        public string Jwt { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
