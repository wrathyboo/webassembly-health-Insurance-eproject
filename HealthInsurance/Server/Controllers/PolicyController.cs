using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Server.Data;
using HealthInsurance.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HealthInsurance.Shared.Dto;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using HealthInsurance.Server.Helpers;

namespace HealthInsurance.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PolicyController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: api/Policy
        [HttpGet]
        [Authorize(Roles = "Administrator, Member")]
        public async Task<ActionResult<IEnumerable<Policy>>> GetPolicies()
        {
            var result = await _context.Policies.Include(i => i.Company).ToListAsync();
			return Ok(Json.ObjectDeserialize(result));
        }

        // GET: api/Policy/Status
        [HttpGet("status")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<Policy>>> GetPolicyApprovals()
        {
            var result = await _context.PolicyApprovalDetails.ToListAsync();
            return Ok(Json.ObjectDeserialize(result));
        }

        // GET: api/Policy/Requests
        [HttpGet("requests")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<PolicyRequest>>> GetPolicyRequests()
        {
            var result = await _context.PolicyRequests.Include(i => i.Company).ToListAsync();
            return Ok(Json.ObjectDeserialize(result));
        }

        // GET: api/Policy/bills
        [HttpGet("bills")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<PolicyTotalDescription>>> GetPolicyBills()
        {
            var result = await _context.PolicyTotalDescriptions.ToListAsync();
            return Ok(Json.ObjectDeserialize(result));
        }

        // GET: api/Policy/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Member")]
        public async Task<ActionResult<Policy>> GetPolicy(int id)
        {
            var policy = await _context.Policies.FindAsync(id);

            if (policy == null)
            {
                return NotFound();
            }

            return policy;
        }

        // GET: api/Policy/request/5
        [HttpGet("request/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<PolicyRequest>> GetRequestPolicy(int id)
        {
            var policy = await _context.PolicyRequests.FindAsync(id);

            if (policy == null)
            {
                return NotFound();
            }

            return policy;
        }

        // PUT: api/Policy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutPolicy(int id, Policy policy)
        {
            if (id != policy.Id)
            {
                return BadRequest();
            }

            _context.Entry(policy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyExists(id))
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

        // POST: //"api/policy/make_request/{currentUser}"
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("make_request")] //Employee will make a request
        [Authorize(Roles = "Administrator, Member")]
        public async Task<ActionResult<PolicyRequest>> PostPolicy(PolicyModel policy)
        {
            var result = await _userManager.FindByNameAsync(policy.EmployeeId);
            if (result == null)
            {
                return NotFound("Username not found!");
            }
            else
            {
                ///Create new request then add to database, Status default "Pending"
                PolicyRequest policyRequest = new PolicyRequest 
                { EmployeeId = result.Id,
                    Status = "Pending",
                    PolicyAmount = policy.Amount,
                   CompanyId = policy.CompanyId, 
                  Emi = policy.Emi,
                 PolicyName = policy.Name};
                await _context.PolicyRequests.AddAsync(policyRequest);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPolicies", new { id = policyRequest.Id }, policyRequest);
            }
            
        }

        // PUT: //"api/policy/request/action/5"
        [HttpPut("request/action/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Policy>> RequestActions(int id, PolicyRequest action)
        {
            var result = await _context.PolicyRequests.FindAsync(id);
            if (result == null)
            {
                return NotFound("Policy request not found!");
            }
            else
            {
                ///If status is "approved", that request will be added to policy database
                if (action.Status.ToLower() == "approved")
                {
                    result.Status = "Approved";
                    Policy policy = new()
                    {
                        Name = result.PolicyName,
                        Amount = result.PolicyAmount,
                        Emi = result.Emi,
                        CompanyId = result.CompanyId
                    };

                    

                    _context.Policies.Add(policy);
                }
                else
                {
                    result.Status = "Denied";
                }
                PolicyApprovalDetails transaction = new()
                {
                    Status = result.Status,
                    RequestId = result.Id,
                    DateApproved = DateTime.UtcNow,
                    Reason = "No context"
                };
                _context.PolicyApprovalDetails.Add(transaction);
                _context.Entry(result).State = EntityState.Modified;

                
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok();
            }

        }

        // DELETE: api/Policy/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        


        private bool PolicyExists(int id)
        {
            return _context.Policies.Any(e => e.Id == id);
        }
    }
}
