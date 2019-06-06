using AppointmentScheduling.DAL;
using AppointmentScheduling.Models;
using AppointmentScheduling.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AppointmentScheduling.Cryptography;

namespace AppointmentScheduling.Controllers
{
    public class PatientController : Controller
    {

        private bool Authorize()
        {
            if (Session["CurrentUser"] == null)
                return false;
            return true;
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
            if (!Authorize())
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
            Patient currentPatient = pdal.Patients.FirstOrDefault<Patient>(x => x.UserName == currentUser.UserName);
            AppointmentDal appDal = new AppointmentDal();
            Appointment update = appDal.Appointments.FirstOrDefault<Appointment>(x => x.Date == chosen.Date && x.DoctorLicense == chosen.DoctorLicense);
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
            Patient currentPatient = pdal.Patients.FirstOrDefault<Patient>(x => x.UserName == currentUser.UserName);
            List<Appointment> appointments = (from app in appDal.Appointments
                                              where app.PatientID == currentPatient.PatientID && DateTime.Compare(DateTime.Now, app.Date) < 0
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
            update.PatientID = null;
            appDal.SaveChanges();
            DoctorDal dctDal = new DoctorDal();
            return Json(new { success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDoctorsByJson()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User currentUser = (User)Session["CurrentUser"];
            DoctorDal docDal = new DoctorDal();
            List<string> doctors = (from doc in docDal.Users
                                    select doc.UserName).ToList<string>();
            Thread.Sleep(1000);
            return Json(doctors, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MassagePage()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return View();
        }
        public ActionResult NewMessage()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return View(new Massage());
        }
        [HttpPost]
        public ActionResult SendMessage()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User CurrentUser = (User)Session["CurrentUser"];
            Massage msg = new Massage
            {
                Read = false,
                date = DateTime.Now,
                SenderUserName = CurrentUser.UserName,
                ReciverUserName = Request.Form["DoctorCombo"],
                msg = Request.Form["msg"]
            };
            TryValidateModel(msg);
            if (ModelState.IsValid)
            {
                MassageDal msgDal = new MassageDal();
                msgDal.Massages.Add(msg);
                msgDal.SaveChanges();
            }
            return View("MassagePage");
        }
        public ActionResult ReciverMessages()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User CurrentUser = (User)Session["CurrentUser"];
            MassageDal msgDal = new MassageDal();
            VMMassages VMm = new VMMassages
            {
                Massages = (from msg in msgDal.Massages
                            where msg.ReciverUserName == CurrentUser.UserName
                            select msg).ToList<Massage>()
            };
            return View(VMm);
        }

        public ActionResult ReadMassage(string sender, DateTime date)
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User CurrentUser = (User)Session["CurrentUser"];
            MassageDal msgDal = new MassageDal();
            Massage m = msgDal.Massages.FirstOrDefault<Massage>(x => x.ReciverUserName == CurrentUser.UserName && x.SenderUserName == sender && x.date == date);
            m.Read = true;
            msgDal.SaveChanges();
            return RedirectToAction("ReciverMessages");
        }
        public ActionResult ShowDetails()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return View();
        }
        public ActionResult ChangePass()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return View(new ChangePassword());
        }
        [HttpPost]
        public ActionResult ChangePassSubmit(ChangePassword pass)
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");

            User currentUser = (User)Session["CurrentUser"];
            AES.Decrypt("9XJTFF0BiA4seHDJ1sIfhg==");
            TryValidateModel(pass);
            if (ModelState.IsValid)
            {
                if (pass.oldPass != AES.Decrypt(currentUser.Password))
                {
                    ViewBag.pass = "old pass doesn't match! Pass hasn't changed";
                    return View("ChangePass");
                }
                UserDal usrDal = new UserDal();
                currentUser = usrDal.Users.FirstOrDefault<User>(x => x.UserName == currentUser.UserName);
                currentUser.Password = AES.Encrypt(pass.newPass);
                ViewBag.pass = "pass has changed";
                return View("ShowDetails");
            }
            return View("ChangePass");
        }
    }
}