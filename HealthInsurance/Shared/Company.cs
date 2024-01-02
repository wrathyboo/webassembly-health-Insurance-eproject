using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? CompanyURL { get; set; }

        public ICollection<Policy> Policies { get; } = new List<Policy>();

        public ICollection<PolicyOnEmployee> PolicyOnEmployees { get; } = new List<PolicyOnEmployee>();
    }
}
