using AppointmentScheduling.DAL;
using AppointmentScheduling.Models;
using AppointmentScheduling.ViewModel;
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
        private void MassagAppointment(string Reciver,string msg)
        {
            Massage newMsg = new Massage
            {
                date = DateTime.Now,
                SenderUserName =((User)Session["CurrentUser"]).UserName,
                ReciverUserName =Reciver,
                Read = false,
                msg = msg
            };
            MassageDal msgDal = new MassageDal();
            msgDal.Massages.Add(newMsg);
            msgDal.SaveChanges();
        }
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
                                              where app.PatientID == null && DateTime.Compare(DateTime.Now, app.Date) < 0
                                              select app).ToList<Appointment>();
            Thread.Sleep(1000);
            return Json(appointments, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChooseAppointment(Appointment chosen)
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            PatientDal pdal = new PatientDal();
            User currentUser = (User)Session["CurrentUser"];
            Patient currentPatient = pdal.Users.FirstOrDefault<Patient>(x=> x.UserName==currentUser.UserName);
            AppointmentDal appDal = new AppointmentDal();
            Appointment update = appDal.Appointments.FirstOrDefault<Appointment>(x => x.Date == chosen.Date && x.DoctorLicense== chosen.DoctorLicense);
            update.PatientID = currentPatient.PatientID;
            appDal.SaveChanges();
            return Json(new { success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult YourAppointments()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return View(new Appointment());
        }
        public ActionResult GetYourAppointmentssByJson()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User currentUser = (User)Session["CurrentUser"];
            AppointmentDal appDal = new AppointmentDal();
            PatientDal pdal = new PatientDal();
            Patient currentPatient = pdal.Users.FirstOrDefault<Patient>(x => x.UserName == currentUser.UserName);
            List<Appointment> appointments = (from app in appDal.Appointments
                                              where app.PatientID == currentPatient.PatientID && DateTime.Compare(DateTime.Now,app.Date)<0
                                              select app).ToList<Appointment>();
            Thread.Sleep(1000);
            return Json(appointments, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CancelAppointment(Appointment chosen)
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            AppointmentDal appDal = new AppointmentDal();
            Appointment update = appDal.Appointments.FirstOrDefault<Appointment>(x => x.Date == chosen.Date && x.DoctorLicense == chosen.DoctorLicense);
            update.PatientID =null;
            appDal.SaveChanges();
            DoctorDal dctDal = new DoctorDal();
            string DoctorUserName = dctDal.Users.FirstOrDefault<Doctor>(x=> x.DoctorLicense==chosen.DoctorLicense).UserName;
            MassagAppointment(DoctorUserName, ((User)Session["CurrentUser"]).UserName.ToString() + " cancel appointment in " + chosen.Date.ToString());
            return Json(new { success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}