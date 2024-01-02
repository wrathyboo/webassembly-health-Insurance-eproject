using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared.Dto
{
	public class PolicyDto
	{
		public int Id { get; set; }
		public string? Name { get; set; }

		public string? Description { get; set; }

		public decimal? Amount { get; set; }

		//Equated Monthly Instalment
		public decimal? Emi { get; set; }

		public int? CompanyId { get; set; }

		public string? CompanyName { get; set; }

		public string? MedicalId { get; set; }

		public string? Status { get; set; }


	}
}
