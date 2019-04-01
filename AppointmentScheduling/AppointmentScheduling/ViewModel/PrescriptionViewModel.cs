using AppointmentScheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentScheduling.ViewModel
{
    public class PrescriptionViewModel
    {
        public Doctor Prescription { get; set; }
        public List<Prescription> Prescriptions { get; set; }
    }
}
