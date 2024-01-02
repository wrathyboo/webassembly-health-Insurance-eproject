using HealthInsurance.Server.Data;
using HealthInsurance.Server.Helpers;
using HealthInsurance.Server.Repository;
using HealthInsurance.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;

namespace HealthInsurance.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Administrator")]  ///Only the admin has access to Employee Manager controller
	public class EmployeeController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IEmployeeRepository employeeRepository)
		{
			_userManager = userManager;
			this.roleManager = roleManager;
			_context = context;
			_employeeRepository = employeeRepository;
		}

        [HttpGet("search")]
        public async Task<IActionResult> GetSearch([FromQuery] SearchParameters searchParameters)
        {
            var result = await _employeeRepository.GetEmployees(searchParameters);
            var employee = new List<EmployeeModel>();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

            //First we get all the users then loop for each user
            foreach (var user in result)
            {
                //Create new list which consist of users that are part of Member role
                var e = new EmployeeModel();

                //GetRolesAsync will return a list of role names assigned from that user
                //If role contains Member, it will initiate a new employee with the given role then add to the list
                var role = await _userManager.GetRolesAsync(user);
                if (role.Contains("Member"))
                {
                    e.UserName = user.UserName;
                    e.Email = user.Email;
                    e.LastName = user.LastName;
                    e.FirstName = user.FirstName;
                    e.PhoneNumber = user.PhoneNumber;
                    e.Address = user.Address;
                    e.Id = user.Id;
                    e.Country = user.Country;
                    e.Role = role.FirstOrDefault();
                    e.CreatedAt = user.CreatedAt;
                    e.Designation = user.Designation;
                    e.Salary = user.Salary;
                    e.PolicyId = user.PolicyId;
                    employee.Add(e);
                }

            }

            return Ok(employee); //Return list of query users that have role "Member"

        }

        // GET: api/Employee
        [HttpGet]
        public async Task<List<EmployeeModel>> GetEmployee()
		{

			var employee = new List<EmployeeModel>();
			var users = _userManager.Users.Include(i => i.PolicyOnEmployee);

			//First we get all the users then loop for each user
			foreach (var user in users)
			{
				//Create new list which consist of users that are part of Member role
				var e = new EmployeeModel();

				//GetRolesAsync will return a list of role names assigned from that user
				//If role contains Member, it will initiate a new employee with the given role then add to the list
				var role = await _userManager.GetRolesAsync(user);
				if (role.Contains("Member"))
				{
					e.UserName = user.UserName;
					e.Email = user.Email;
					e.LastName = user.LastName;
					e.FirstName = user.FirstName;
					e.PhoneNumber = user.PhoneNumber;
					e.Address = user.Address;
					e.Id = user.Id;
					e.Country = user.Country;
					e.Role = role.FirstOrDefault();
					e.CreatedAt = user.CreatedAt;
					e.Designation = user.Designation;
					e.Salary = user.Salary;
					e.PolicyId = user.PolicyId;
					employee.Add(e);
				}

			}

			return employee; //Return list of users that have role "Member"
		}

        // GET: api/Employee/5
        [HttpGet("{id}")]
		public async Task<ActionResult<ApplicationUser>> GetEmployeeById(string id)
		{
			var result = await _userManager.FindByIdAsync(id);
			if (result == null)
			{
				return NotFound("No user found!");
			}
			else return Ok(result);
		}

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeModel>> DeleteEmployeeById(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
			
			///To delete an employee, we will need to check first if they currently have an attached policy
			if (result.PolicyId != null)
			{
                var policyOnEmployee = await _context.PolicyOnEmployees.FindAsync(result.PolicyId!);
				///Remove policy on employee
                _context.PolicyOnEmployees.Remove(policyOnEmployee!);
				await _context.SaveChangesAsync();
            }

			
			if (result == null)
			{
				return NotFound("No user found!");
			}
			else
			{
				///Finially, delete employee
				await _userManager.DeleteAsync(result);
				return Ok(result);
			}
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateEmployee(string id, EmployeeModel newEmployeeInfo)
        {
            var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("No user found!");
			}
			else
			{
				user.UserName = newEmployeeInfo.UserName;
				user.Email = newEmployeeInfo.Email;
				user.FirstName = newEmployeeInfo.FirstName;
				user.LastName = newEmployeeInfo.LastName;
				user.PhoneNumber = newEmployeeInfo.PhoneNumber;
				user.Address = newEmployeeInfo.Address;
				user.City = newEmployeeInfo.City;
				user.Country = newEmployeeInfo.Country;
				user.State = newEmployeeInfo.State;
				user.Salary = newEmployeeInfo.Salary;
				user.Designation = newEmployeeInfo.Designation;


                await _userManager.UpdateAsync(user);

				return Ok("Employee updated!");
			}
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeModel model)
        {
			//Check if username and email already exist
			var userCheck = await _userManager.FindByNameAsync(model!.UserName!) ?? await _userManager.FindByEmailAsync(model!.Email!); ;
			if (userCheck != null) 
			{
				return new CustomError(HttpStatusCode.BadRequest, "Username or email already exist");
            }

            var newEmp = new ApplicationUser{
				UserName = model.UserName,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Address = model.Address,
				City = model.City,
				Country = model.Country,
				State = model.State,
				Salary = model.Salary,
				Designation = model.Designation,
				PhoneNumber = model.PhoneNumber,
			};
			///Add employee
            var result = await _userManager.CreateAsync(newEmp, model.Password);
			///Default assign their role to "Member"
            var assignRole = await _userManager.AddToRoleAsync(newEmp, "Member");
            if (result.Succeeded && assignRole.Succeeded)
            {
                return Ok("New user successfully created!");
            }
            else return StatusCode(400, "Something wrong");
        }


    }
}
