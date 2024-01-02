using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class PolicyOnEmployee
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public decimal? Amount { get; set;}

        public decimal? Emi {  get; set; }

        public DateTime? PolicyStartDate { get; set;}

        public DateTime? PolicyEndDate { get; set;}

        public int? Duration { get; set;}

        public int? CompanyId { get; set; }

        public string? MedicalId { get; set; }

        public string? EmployeeId { get; set; }

        public Company? Company { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
