using AppointmentScheduling.DAL;
using AppointmentScheduling.Models;
using AppointmentScheduling.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointmentScheduling.Controllers
{
    public class NotificationController : Controller
    {
        private bool Authorize()
        {
            if (Session["CurrentUser"] == null)
                return false;
            else
                return true;
        }

        public ActionResult ShowNotification()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User currentUser = (User)Session["CurrentUser"];
            VMMassages massages = new VMMassages();
            MassageDal msgDal = new MassageDal();
            massages.Massages = (from msg in msgDal.Massages
                                  where msg.ReciverUserName == currentUser.UserName
                                  select msg).ToList<Massage>();
            foreach (Massage msg in massages.Massages)
                msg.Read = true;
            msgDal.SaveChanges();
            return View(massages);
        }
    }
}