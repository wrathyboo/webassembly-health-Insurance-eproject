using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class Policy
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Amount { get; set; }

        //Equated Monthly Instalment
        public decimal? Emi { get; set; }

        public int? CompanyId { get; set; }

        public string? MedicalId { get; set; }

        public Company? Company { get; set; }
    }
}
