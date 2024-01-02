using HealthInsurance.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Server.Helpers;
using HealthInsurance.Server.Data;

namespace HealthInsurance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator, Member")] //Only admin and Employee have access to this controller
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            _context = context;
        }

        // GET: api/User/{name}
        [HttpGet("{username}")]
        public async Task<ActionResult<ApplicationUser>> GetUserDetails(string username)
        {
            //Find user including policy attached
            var result = await _userManager.Users.Include(i => i.PolicyOnEmployee).FirstOrDefaultAsync(x => x.UserName == username);
            return Ok(Json.SingularObjectDeserialize(result));
        }

        // POST: api/User/change-password/{name}
        [HttpPost("change-password/{username}")]
        public async Task<ActionResult<string>> PostChangePassword(string username, PasswordModel password)
        {
            ///Check for matching Username
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound("No user found!");
            }
            else
            {
                ///For simplicity, we will generate a password reset token for user
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                ///We will assume the user has recieved confirmation email, hence the token
                var result = await _userManager.ResetPasswordAsync(user, token, password!.Password!);

                if (result.Succeeded)
                {
                    return Ok("Successfully reset password!");
                }
                else
                {
                    return "An error occured";
                }
            }
        }

        // POST: api/User/order
        [HttpPost("order")]
        public async Task<ActionResult<PolicyOrder>> PostOrderPolicy(PolicyOrder order)
        {
            ///Get policies included company details
            var policy = await _context.Policies.Include(i => i.Company).FirstOrDefaultAsync(x => x.Id == order.Id);

            ///Deserialize to handle reference
            Json.SingularObjectDeserialize(policy);

            PolicyTotalDescription transaction = new() //create new policy total description
            {
                Name = policy.Name,
                Description = policy.Description,
                Amount = (decimal)policy.Amount,
                PolicyDurationInMonths = 0,
                CompanyName = policy.Company.Name,
                MedicalId = policy.MedicalId
            };
            await _context.PolicyTotalDescriptions.AddAsync(transaction);
            await _context.SaveChangesAsync();


            return Ok(transaction);
        }

        // DELETE: api/User/policy/5
        [HttpDelete("policy/{id}")] ///Remove existing policy on employee
        public async Task<IActionResult> Delete(int id)
        {
            var policyOnEmployee = await _context.PolicyOnEmployees.FindAsync(id);

            if (policyOnEmployee == null)
            {
                return NotFound();
            }
            ///After remove employee policy, we must also update PolicyId field in User table
            var user = await _userManager.FindByIdAsync(policyOnEmployee!.EmployeeId!);
            _context.PolicyOnEmployees.Remove(policyOnEmployee);
            await _context.SaveChangesAsync();

            user.PolicyId = null;
            await _userManager.UpdateAsync(user);


            return NoContent();
        }
    }
}
