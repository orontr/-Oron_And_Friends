using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppointmentScheduling.Models
{
    public class Doctor {
        [Key]
        [Required]
        [RegularExpression("^[1-9]{5}$", ErrorMessage = "Doctor License must be 5 digits (digits 1-9)")]
        public string DoctorLicense { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "Doctor Id must be 9 digits (digits 0-9)")]
        public string DoctorID { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string DoctorEmail { get; set; }

        [Required]
        public string DoctorSpecialization { get; set; }

        [Required]
        public string UserName { get; set; }

    }
}