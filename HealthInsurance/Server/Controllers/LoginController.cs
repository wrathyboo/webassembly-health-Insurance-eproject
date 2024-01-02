using HealthInsurance.Server.Data;
using HealthInsurance.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthInsurance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase //Any user can access the Login controller
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(IConfiguration configuration,
                               SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        // POST: api/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            ///Upon signing in
            var result = await _signInManager.PasswordSignInAsync(login.Username!, login.Password!, false, false);

            ///If either password or username not correct, return BadRequest
            if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "Username or password are invalid." });
            ///Else, prepare claim token for user
            var user = await _signInManager.UserManager.FindByNameAsync(login.Username!); //Get user details
            var roles = await _signInManager.UserManager.GetRolesAsync(user!); //Get list of roles assigned to that user
            var claims = new List<Claim>
            {
                ///Add username claim
                new Claim(ClaimTypes.Name, login.Username!)
            };

            foreach (var role in roles)
            {
                ///Add roles to claims
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            ///Our secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]!));
            ///Credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            ///Expiry date from created date + 1 day (Can be config in LaunchSettings.json)
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));
            ///Generate JWT token
            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            ///Successful login returns a JWT token
            return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
