using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HealthInsurance.Shared
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? Designation {  get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Salary { get; set; }

        public int? PolicyId { get; set; }

        public PolicyOnEmployee? PolicyOnEmployee { get; set; }

        public ICollection<PolicyRequest>? PolicyRequests { get; } = new List<PolicyRequest>();
    }
}
