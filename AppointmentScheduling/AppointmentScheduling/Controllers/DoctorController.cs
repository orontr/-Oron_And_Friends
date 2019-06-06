using AppointmentScheduling.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AppointmentScheduling.Models;
using AppointmentScheduling.ViewModel;

namespace AppointmentScheduling.Controllers
{
    public class DoctorController : Controller
    {
        /////////////////////////Appointments/////////////////////////

        public ActionResult GetAppointmentsByJson()
        {
            AppointmentDal appDal = new AppointmentDal();
            Thread.Sleep(3000);
            List<Appointment> objAppointments = appDal.Appointments.ToList<Appointment>();

            return Json(objAppointments, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        //AddPerfume is addressing to the form to get data from the user
        public ActionResult AddAppointment()
        {
            AppointmentDal appDal = new AppointmentDal();
            List<Appointment> objAppointment = appDal.Appointments.ToList<Appointment>();
            AppointmentViewModel cvm = new AppointmentViewModel();
            cvm.Appointment = new Appointment();
            cvm.Appointments = objAppointment;

            return View("AddAppointment", cvm);
        }

        //we get the data from the form that the user filled (Product.cshtml) the data is passed to the Product controller
        //and the object that was created (cust) we send to Product view (strongly typed view-works with Product model)
        /*we can pass an object to the function instead of writing all the things below and that because all the named in the html page are identical to fields in the Product model/class */


        [HttpPost]
        public ActionResult SubmitAppointment()
        {
            Appointment objAppointment = new Appointment();
            objAppointment.PatientID = Request.Form["Appointment.PatientID"].ToString();
            objAppointment.Date =Convert.ToDateTime(Request.Form["Appointment.Date"].ToString());
            objAppointment.DoctorLicense = Request.Form["Appointment.DoctorLicense"].ToString();


            AppointmentDal appDal = new AppointmentDal();

            if (ModelState.IsValid)
            {
                try
                {

                    appDal.Appointments.Add(objAppointment);//in memory adding only
                    appDal.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["error"] = "The employee already exist!\n"; // print error message
                    return View();
                }

            }
            List<Appointment> objAppointments = appDal.Appointments.ToList<Appointment>();
            Thread.Sleep(3000);
            return Json(objAppointments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowAppointmentSearch()
        {
            AppointmentViewModel cvm = new AppointmentViewModel();
            cvm.Appointments = new List<Appointment>();
            return View("SearchAppointment", cvm);//pass model cvm to SearhAppointment cshtml 
        }

        public ActionResult SearchAppointment()
        {
            AppointmentDal appDal = new AppointmentDal();
            string searchValue = Request.Form["txtPatientID"].ToString();
            List<Appointment> objAppointments = (from x in appDal.Appointments
                                                 where x.PatientID.Contains(searchValue)
                                                 select x).ToList<Appointment>();
            AppointmentViewModel cvm = new AppointmentViewModel();
            cvm.Appointments = objAppointments;

            return View(cvm);


        }

        /////////////////////////Referral///////////////////////////////////////////
        public ActionResult GetReferralsByJson()
        {
            ReferralDal refDal = new ReferralDal();
            Thread.Sleep(3000);
            List<Referral> objReferrals = refDal.Referrals.ToList<Referral>();

            return Json(objReferrals, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //AddPerfume is addressing to the form to get data from the user
        public ActionResult AddReferral()
        {
            ReferralDal refDal = new ReferralDal();
            List<Referral> objReferrals = refDal.Referrals.ToList<Referral>();
            ReferralViewModel cvm = new ReferralViewModel();
            cvm.Referral = new Referral();
            cvm.Referrals = objReferrals;

            return View("AddDocReferral", cvm);
        }

        //we get the data from the form that the user filled (Product.cshtml) the data is passed to the Product controller
        //and the object that was created (cust) we send to Product view (strongly typed view-works with Product model)
        /*we can pass an object to the function instead of writing all the things below and that because all the named in the html page are identical to fields in the Product model/class */


        [HttpPost]
        public ActionResult SubmitReferral()
        {
            Referral objReferral = new Referral();
            objReferral.PatientID = Request.Form["Referral.PatientID"].ToString();
            objReferral.IssueDate = Convert.ToDateTime(Request.Form["Referral.IssueDate"].ToString());
            objReferral.ExpDate = Convert.ToDateTime(Request.Form["Referral.ExpDate"].ToString());
            objReferral.ReferralCode = Request.Form["Referral.ReferralCode"].ToString();
            objReferral.DoctorLicense = Request.Form["Referral.DoctorLicense"].ToString();

            ReferralDal refDal = new ReferralDal();

            if (ModelState.IsValid)
            {
                try
                {

                    refDal.Referrals.Add(objReferral);//in memory adding only
                    refDal.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["error"] = "There is already a valid referral to this patient!\n"; // print error message
                    return View();
                }

            }
            List<Referral> objReferrals = refDal.Referrals.ToList<Referral>();
            Thread.Sleep(3000);
            return Json(objReferrals, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowReferralSearch()
        {
            ReferralViewModel cvm = new ReferralViewModel();
            cvm.Referrals = new List<Referral>();
            return View("SearchReferral", cvm);//pass model cvm to SearhReferral cshtml 
        }

        public ActionResult SearchReferral()
        {
            ReferralDal refDal = new ReferralDal();
            string searchValue = Request.Form["txtPatientID"].ToString();
            List<Referral> objReferrals = (from x in refDal.Referrals
                                           where x.PatientID.Contains(searchValue)
                                           select x).ToList<Referral>();
            ReferralViewModel cvm = new ReferralViewModel();
            cvm.Referrals = objReferrals;

            return View(cvm);


        }


        /////////////////////////Prescription///////////////////////////////////////////
        public ActionResult GetPrescriptionsByJson()
        {
            PrescriptionDal Presdal = new PrescriptionDal();
            Thread.Sleep(3000);
            List<Prescription> objPrescriptions = Presdal.Prescriptions.ToList<Prescription>();

            return Json(objPrescriptions, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        //AddPerfume is addressing to the form to get data from the user
        public ActionResult AddPrescription()
        {
            PrescriptionDal Presdal = new PrescriptionDal();
            List<Prescription> objPrescriptions = Presdal.Prescriptions.ToList<Prescription>();
            PrescriptionViewModel cvm = new PrescriptionViewModel();
            cvm.Prescription = new Prescription();
            cvm.Prescriptions = objPrescriptions;

            return View("AddDocPrescription", cvm);
        }

        //we get the data from the form that the user filled (Product.cshtml) the data is passed to the Product controller
        //and the object that was created (cust) we send to Product view (strongly typed view-works with Product model)
        /*we can pass an object to the function instead of writing all the things below and that because all the named in the html page are identical to fields in the Product model/class */


        [HttpPost]
        public ActionResult SubmitSup()
        {
            Prescription objPrescription = new Prescription();
            objPrescription.PatientID = Request.Form["Prescription.PrescriptionId"].ToString();
            objPrescription.IssueDate = Convert.ToDateTime(Request.Form["Prescription.PrescriptionName"].ToString());
            objPrescription.ExpDate = Convert.ToDateTime(Request.Form["Prescription.PrescriptionEmail"].ToString());
            objPrescription.Medication = Request.Form["Prescription.PrescriptionAddress"].ToString();
            objPrescription.DoctorLicense = Request.Form["Prescription.PrescriptionPhoneNumber"].ToString();

            PrescriptionDal Presdal = new PrescriptionDal();

            if (ModelState.IsValid)
            {
                try
                {

                    Presdal.Prescriptions.Add(objPrescription);//in memory adding only
                    Presdal.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["error"] = "The supplier already exist!\n"; // print error message
                    return View();
                }

            }
            List<Prescription> objPrescriptions = Presdal.Prescriptions.ToList<Prescription>();
            Thread.Sleep(3000);
            return Json(objPrescriptions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPrescriptionSearch()
        {
            PrescriptionViewModel cvm = new PrescriptionViewModel();
            cvm.Prescriptions = new List<Prescription>();
            return View("SearchPrescription", cvm);//pass model cvm to SearhEmployee cshtml 
        }

        public ActionResult SearchPrescription()
        {
            PrescriptionDal Presdal = new PrescriptionDal();
            string searchValue = Request.Form["txtPrescriptionName"].ToString();
            List<Prescription> objPrescriptions = (from x in Presdal.Prescriptions
                                                   where x.PatientID.Contains(searchValue)
                                                   select x).ToList<Prescription>();
            PrescriptionViewModel cvm = new PrescriptionViewModel();
            cvm.Prescriptions = objPrescriptions;

            return View(cvm);


        }

    }




}