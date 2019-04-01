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
        public string DoctorLicenese { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string DoctorID { get; set; }
        [Required]
        public string DoctorEmail { get; set; }
        [Required]
        public string DoctorSpecialization { get; set; }
        [Required]
        public string UserName { get; set; }

    }
}