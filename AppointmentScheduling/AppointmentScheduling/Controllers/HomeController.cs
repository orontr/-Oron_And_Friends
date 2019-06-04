using AppointmentScheduling.DAL;
using AppointmentScheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppointmentScheduling.viewModel;
using AppointmentScheduling.Classes;

namespace AppointmentScheduling.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult RedirectByUser()
        {
            if (Session["CurrentUser"] != null)
            {
                User currentUsr = (User)(Session["CurrentUser"]);
                if (currentUsr.UserType)
                    return RedirectToAction("DoctorPage", "Doctor");
                else
                    return RedirectToAction("PatientPage", "Patient");
            }
            else
            {
                TempData["notAuthorized"] = "You have no permission!";
                return RedirectToAction("HomePage");
            }
        }


        public ActionResult HomePage()
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            return View();
        }
        public ActionResult LoginPage()
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            return View(new UserLogin());
        }
        [HttpPost]
        public ActionResult Login(UserLogin usr)
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            if (ModelState.IsValid)
            {
                UserDal usrDal = new UserDal();
                string decryptedPassword;
                User objUser = (from user in usrDal.Users
                                where user.UserName == usr.UserName
                                select user).FirstOrDefault<User>();
                if (objUser == null || Cryptography.Decrypt(objUser.Password) != usr.Password)
                {
                    ViewBag.errorUserLogin = "UserName or Password are incorrect";
                    return View("LoginPage", usr);
                }
                objUser.Password = "";
                Session["CurrentUser"] = objUser;
                return RedirectToAction("RedirectByUser");
            }
            else
            {
                usr.Password = "";
                return View("LoginPage", usr);
            }
        }


        public ActionResult SignupPage()
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            return View(new VMUserRegister());
        }

        public ActionResult Register()
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            VMUserRegister newUsr = new VMUserRegister();
            newUsr.NewUser = new User();
            return View("SignupPage", newUsr);
        }

        [HttpPost]
        public ActionResult RegisterCon(VMUserRegister usr)
        {
            string encryptedPassword = Cryptography.Encrypt(usr.Password);
            string encryptedAnswer = Cryptography.Encrypt(usr.NewUser.SecurityAnswer);

            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");

            usr.NewUser.UserName = usr.UserName;
            usr.NewUser.Password = usr.Password;
            //usr.NewUser.SecurityQuestion=usr.Security
            //usr.NewUser = usr.Email;
            ModelState.Clear();
            TryValidateModel(usr);
            if (ModelState.IsValid)
            {
                UserDal usrDal = new UserDal();

                User objUser = (from user in usrDal.Users
                                where user.UserName == usr.NewUser.UserName
                                select user).FirstOrDefault<User>();
                if (objUser != null)
                {
                    ViewBag.errorUserRegister = "The user name is already exist";
                    return View("SignupPage", usr);
                }
                usr.NewUser.UserType = false;

                //usr.NewUser.Password = encryptedPassword;
                usr.NewUser.SecurityAnswer = encryptedAnswer;
                usr.NewUser.SecurityQuestion = Request.Form["sq"];

                usrDal.Users.Add(new User {UserName=usr.UserName, Password=encryptedPassword, SecurityAnswer=encryptedAnswer, SecurityQuestion=usr.NewUser.SecurityQuestion, UserType=false });
                usrDal.SaveChanges();
                ViewBag.registerSuccessMsg = "The registration succeded!";
                return View("HomePage", usr.NewUser);
            }
            else
            {
                usr.Password = "";
                return View("SignupPage", usr);
            }
        }
    }
}