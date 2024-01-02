using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInsurance.Shared
{
    public class EmployeeModel
    {
        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required]
        [RegularExpression(@"^\S+@\S+$", ErrorMessage ="Invalid Email")]
        public string? Email { get; set; }

        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage ="Must contain at least 1 lowercase, 1 uppercase and 1 number")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        [StringLength(14, ErrorMessage = "The {0} must be at least {2} numbers long.", MinimumLength = 6)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? State { get; set;}

        [Required]
        public string? Country { get; set; }

        [Required]
        public string? City { get; set; }

        public string? Role { get; set; }

        public string? Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public string? Designation {  get; set; }

        [Required]
        public decimal Salary { get; set; }

        public int? PolicyId { get; set; }

        public string? PolicyName { get; set; }
    }
}
