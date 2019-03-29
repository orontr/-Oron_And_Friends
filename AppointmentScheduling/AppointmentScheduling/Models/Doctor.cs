using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppointmentScheduling.Models
{
    public class Doctor {
        [Required]
        public string id { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}