using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class PolicyApprovalDetails
    {
        public int Id { get; set; }
        public int RequestId { get; set; }

        public DateTime DateApproved {  get; set; }

        public string? Status { get; set; }

        public string? Reason { get; set; }
    }
}
