using HealthInsurance.Server.Paging;
using HealthInsurance.Shared;

namespace HealthInsurance.Server.Repository
{
    public interface IEmployeeRepository
    {
        Task<PagedList<ApplicationUser>> GetEmployees(SearchParameters searchParameters);

    }
}
