using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class PolicyModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Amount { get; set; }

        //Equated Monthly Instalment
        public decimal? Emi { get; set; }

        public int? CompanyId { get; set; }

        public string? MedicalId { get; set; }

        public string? Status { get; set; }

        public DateTime? PolicyStartDate { get; set; }

        public DateTime? PolicyEndDate { get; set; }

        public int RequestId { get; set; }

        public DateTime DateApproved { get; set; }

        public string? Reason { get; set; }

        public string? EmployeeId { get; set;}
    }
}
