using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class HospitalDetails
    {
        [Display(Name = "Medical ID")]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Location { get; set; }

        public string Url { get; set; }
    }
}
