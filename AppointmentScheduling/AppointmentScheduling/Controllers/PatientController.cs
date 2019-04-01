using AppointmentScheduling.DAL;
using AppointmentScheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            return View(new Appointment());
        }

        public ActionResult GetAppointmentsVacantByJson()
        {
            if (Session["CurrentUser"] == null)
                return RedirectToAction("RedirectByUser", "Home");
            AppointmentDal appDal = new AppointmentDal();
            List<Appointment> appointments = (from app in appDal.Appointments
                                              where app.PatientID == null
                                              select app).ToList<Appointment>();
            Thread.Sleep(1000);
            return Json(appointments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult chooseAppointment(string e)
        {
            return View("AppointmentScheduling");
        }
    }
}