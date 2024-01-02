using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class PolicyRequest
    {
        public int Id {  get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? EmployeeId { get; set; }

        public string? PolicyName { get; set; }

        public decimal? PolicyAmount { get; set; }

        public decimal? Emi {  get; set; }

        public int? CompanyId { get; set; }

        public string? Status { get; set; }

        public Company? Company { get; set; }

        [JsonIgnore]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
