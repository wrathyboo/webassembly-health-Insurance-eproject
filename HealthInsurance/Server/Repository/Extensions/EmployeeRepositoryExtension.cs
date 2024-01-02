using HealthInsurance.Shared;

namespace HealthInsurance.Server.Repository.Extensions
{
    public static class EmployeeRepositoryExtension
    {
        ///A static class with a single static method that extends the IQueryable<Product> type and accepts a single string parameter
        public static IQueryable<ApplicationUser> Search(this IQueryable<ApplicationUser> products, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();
            return products.Where(p => p.FirstName.ToLower().Contains(lowerCaseSearchTerm) || p.LastName.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
