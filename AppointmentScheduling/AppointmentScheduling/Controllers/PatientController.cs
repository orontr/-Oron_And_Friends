using AppointmentScheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointmentScheduling.Controllers
{
    public class PatientController : Controller
    {
        private bool Authorize()
        {
            if (Session["CurrentUser"] == null)
                return false;
            else
                return (((User)Session["CurrentUser"]).UserType == false);
        }
        public ActionResult PatientPage()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return View();
        }
        public ActionResult AppointmentScheduling()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");

            return View();
        }

    }
}