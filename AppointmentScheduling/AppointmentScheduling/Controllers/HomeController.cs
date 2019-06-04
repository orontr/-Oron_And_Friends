﻿using AppointmentScheduling.DAL;
using AppointmentScheduling.Models;
using AppointmentScheduling.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                TempData["notAuthorized"] = "אין הרשאה!";
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

                User objUser = (from user in usrDal.Users
                                where user.UserName == usr.UserName
                                select user).FirstOrDefault<User>();
                if (objUser == null || objUser.Password != usr.Password)
                {
                    ViewBag.errorUserLogin = "UserName or Password incorrect";
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


        public ActionResult Signin()
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            return View();
        }

        public ActionResult Signup()
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            return View(new VMUserRegister());
        }
        [HttpPost]
        public ActionResult RegisterSubmit(VMUserRegister usr)
        {

            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            usr.NewUser.Password = usr.Password;
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
                    ViewBag.errorUserRegister = "שם המשתמש שבחרת קיים";
                    return View("Signup", usr);
                }
                usr.NewUser.UserType = false;
                usrDal.Users.Add(usr.NewUser);
                usrDal.SaveChanges();
                ViewBag.registerSuccessMsg = "ההרשמה בוצעה בהצלחה!";
                return View("HomePage", usr.NewUser);
            }
            else
            {
                usr.Password = "";
                return View("Register", usr);
            }
        }

        public ActionResult Logout()
        {
            if (Session["CurrentUser"] == null)
                return RedirectToAction("RedirectByUser");

            Session["CurrentUser"] = null;
            return RedirectToAction("RedirectByUser");
        }
    }
}