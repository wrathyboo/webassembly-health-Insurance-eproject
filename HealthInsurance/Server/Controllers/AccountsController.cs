using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HealthInsurance.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using HealthInsurance.Server.Data;
using HealthInsurance.Server.Helpers;

namespace HealthInsurance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationUser>>> Get()
        {
            var users = await _userManager.Users
                .ToListAsync();
            return users;
        }

        [HttpGet("find-user/{username}")]
        public async Task<ActionResult<ApplicationUser>> GetByNameOrEmail(string username)
        {
            var result = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(username);
           
            if (result != null)
            {
                return Ok(Json.SingularObjectDeserialize(result));
            }
            else
            {
                return NotFound("No matching username or email found!");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var newUser = new ApplicationUser { UserName = model.Username, Email = model.Email };

            //Create user
            var result = await _userManager.CreateAsync(newUser, model.Password!);
            
            if (result.Succeeded)
            {
                if (model.Role == null)
                {
                    var assignRoleDefault = await _userManager.AddToRoleAsync(newUser, "User");
                }
                else
                {
                    var assignRoles = await _userManager.AddToRoleAsync(newUser, model.Role);
                }
                //After new user is successfully created, we can assign their role default as a Member
                
                return Ok(new RegisterResult { Successful = true });
               
            }
            else
            {
                var errors = result.Errors.Select(x => x.Description);
                return Ok(new RegisterResult { Successful = false, Errors = errors });
            }
        }
    }
}
