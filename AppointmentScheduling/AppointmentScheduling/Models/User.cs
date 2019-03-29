using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppointmentScheduling.Models
{
    public class User
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}