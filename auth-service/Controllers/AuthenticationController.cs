using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using auth_service.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace auth_service.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationContext _context;

        public AuthenticationController(AuthenticationContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostLogin(LoginRequest loginRequest)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == loginRequest.Username);

            if (user == null)
            {
                return Unauthorized();
            }

            if (user.Password != loginRequest.Password)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>()
            {
                new Claim("UserId", user.UserId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);
            HttpContext.User = claimsPrinciple;

            await HttpContext.AuthenticateAsync();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrinciple);

            return Ok();
        }

        [HttpGet("token")]
        public IActionResult GetToken()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var claims = HttpContext.User.Claims;
            var userId = claims.Single(c => c.Type == "UserId").Value;

            const string jwtSecret = "asdv234234^&%&^%&^hjsdfb2%%%";
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", userId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateToken(descriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);

            return Ok(new TokenResponse()
            {
                Jwt = jwtTokenString
            });
        }
    }

    public class TokenResponse
    {
        public string Jwt { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}