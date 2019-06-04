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
<<<<<<< HEAD
        public SecurityQuestion SecurityQuestion { get; set; }
=======
        public string SecurityQuestion { get; set; }
        [Required]
        public string SecurityAnswer { get; set; }
        private string privateKEY=(new Random()).ToString();
>>>>>>> 7b3e7d8fdf91a735d7c1ebafb4024f7ed5d98ca2

        public void EncryptQes()
        {

        }
    }
}