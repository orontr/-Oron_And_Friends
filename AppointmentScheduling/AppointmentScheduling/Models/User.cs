using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppointmentScheduling.Models
{
    public class User:UserLogin
    {

        public  bool UserType { get; set; }
        [Required]
        public string SecurityQuestion { get; set; }
        [Required]
        public string SecurityAnswer { get; set; }
        private string privateKEY=(new Random()).ToString();

        public void EncryptQes()
        {

        }
    }
}