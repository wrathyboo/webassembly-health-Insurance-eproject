using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Server.Data;
using HealthInsurance.Shared;
using Microsoft.AspNetCore.Identity;
using HealthInsurance.Server.Helpers;
using HealthInsurance.Shared.Dto;
using Microsoft.AspNetCore.Authorization;

namespace HealthInsurance.Server.Controllers
{
    [Route("api/policy-employee")]
    [ApiController]
    public class PolicyOnEmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PolicyOnEmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/PolicyOnEmployee
        [HttpGet]
        [Authorize(Roles = "Administrator, Member")]
        public async Task<ActionResult<IEnumerable<PolicyOnEmployee>>> GetPolicyOnEmployees()
        {
            return await _context.PolicyOnEmployees.ToListAsync();
        }

        // GET: api/PolicyOnEmployee/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Member")]
        public async Task<ActionResult<PolicyEmployeeDto>> GetPolicyOnEmployee(string id)
        {
            var policyOnEmployee = await _userManager.Users.Include(i => i.PolicyOnEmployee).FirstOrDefaultAsync(x => x.Id == id);

            if (policyOnEmployee == null)
            {
                return NotFound();
            }
            policyOnEmployee = Json.SingularObjectDeserialize(policyOnEmployee);

            ///Transfer to DTO
            PolicyEmployeeDto dto = new()
            {
                PolicyId = policyOnEmployee.PolicyId,
                Name = policyOnEmployee.PolicyOnEmployee?.Name,
                FirstName = policyOnEmployee.FirstName,
                LastName = policyOnEmployee.LastName,
                Amount = policyOnEmployee.PolicyOnEmployee?.Amount,
                Emi = policyOnEmployee.PolicyOnEmployee?.Emi,
                PolicyEndDate = policyOnEmployee.PolicyOnEmployee?.PolicyEndDate,
                PolicyStartDate = policyOnEmployee.PolicyOnEmployee?.PolicyStartDate,
                Duration = policyOnEmployee.PolicyOnEmployee?.Duration,
                EmployeeId = policyOnEmployee.Id,
                CompanyId = policyOnEmployee.PolicyOnEmployee?.CompanyId,
                MedicalId = policyOnEmployee.PolicyOnEmployee?.MedicalId,
            };


            return dto;
        }

        // PUT: api/PolicyOnEmployee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPolicyOnEmployee(int id, PolicyOnEmployee policyOnEmployee)
        {
            if (id != policyOnEmployee.Id)
            {
                return BadRequest();
            }

            _context.Entry(policyOnEmployee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyOnEmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: api/PolicyOnEmployee/5/1
        [HttpGet("{id}/{policyId}")]
        [Authorize(Roles = "Administrator, Member")]
        public async Task<ActionResult<PolicyOnEmployee>> PostPolicyOnEmployee(string id, int policyId)
        {
            var user = await _userManager.FindByIdAsync(id);
            var policyOnEmployee = await _context.Policies.FindAsync(policyId);
            PolicyOnEmployee policy = new() //create new policy for employee
            {
                Name = policyOnEmployee.Name,
                Amount = policyOnEmployee.Amount,
                Emi = policyOnEmployee.Emi,
                PolicyStartDate = DateTime.Now, PolicyEndDate = DateTime.Now,
                Duration = 0,
                CompanyId = policyOnEmployee.CompanyId,
                MedicalId = policyOnEmployee.MedicalId,
                EmployeeId = id
            };
            if (user == null)
            {
                return NotFound("No user found!");
            }
            else
            {
                //Add to database
                await _context.PolicyOnEmployees.AddAsync(policy);
                await _context.SaveChangesAsync();

                user.PolicyId = policy.Id;
                await _userManager.UpdateAsync(user);
            }
            return Ok($"Successfully assigned policy to user {id}");
        }
 

        private bool PolicyOnEmployeeExists(int id)
        {
            return _context.PolicyOnEmployees.Any(e => e.Id == id);
        }
    }
}
