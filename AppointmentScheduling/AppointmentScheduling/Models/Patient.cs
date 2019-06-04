
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppointmentScheduling.Models
{
    public class Patient
    {
        [Key]
        [Required(ErrorMessage = "Required field")]
        [RegularExpression("[0-9]+$", ErrorMessage = "מספרים בלבד")]
        [StringLength(9, ErrorMessage = "9 ספרות בדיוק")]
        public string PatientID { get; set; }
        [Required(ErrorMessage = "Required field")]
        [RegularExpression("[a-zA-Z]+$", ErrorMessage = "אותיות באנגלית בלבד")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required field")]
        [RegularExpression("[a-zA-Z]+$", ErrorMessage = "אותיות באנגלית בלבד")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Enter your b.Date")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage ="Enter your email")]
        public string PatientEmail { get; set; }
        public string UserName { get; set; }
    }
}