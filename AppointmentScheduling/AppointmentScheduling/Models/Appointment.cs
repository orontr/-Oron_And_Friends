using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AppointmentScheduling.Models
{
    public class Appointment
    {
        
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "Doctor License must be 9 digits (digits 0-9)")]
        public string PatientID { get; set; }
        [Key, Column(Order = 0)]
        [Required]
        public DateTime Date { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Doctor License must be 5 digits (digits 0-9)")]
        public string DoctorLicense { get; set; }
    }
}