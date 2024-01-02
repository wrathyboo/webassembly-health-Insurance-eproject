using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class PolicyTotalDescription
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public int? PolicyDurationInMonths { get; set; }

        public string? CompanyName { get; set; }

        public string? MedicalId { get; set; }


    }
}
