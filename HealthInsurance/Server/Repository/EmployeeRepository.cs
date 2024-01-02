using HealthInsurance.Shared;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Server.Paging;
using HealthInsurance.Server.Data;
using Microsoft.AspNetCore.Identity;
using HealthInsurance.Server.Repository.Extensions;

namespace HealthInsurance.Server.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly UserManager<ApplicationUser> _context;

        public EmployeeRepository(UserManager<ApplicationUser> context)
        {
            _context = context;
        }
        public async Task<PagedList<ApplicationUser>> GetEmployees(SearchParameters searchParameters)
        {
            var products = await _context.Users.Search(searchParameters.SearchTerm).Include(i => i.PolicyOnEmployee).ToListAsync();
              
            return PagedList<ApplicationUser>
                .ToPagedList(products, searchParameters.PageNumber, searchParameters.PageSize);
        }
    }
}
