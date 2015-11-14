using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class WarmUpController : Controller
    {
        public PartialViewResult TestMail()
        {
            ViewData["Controller_Name"] = "WarmUp";
            new Utilities.Email.Invoke((new Utilities.Email()).SendMail).BeginInvoke("mehul.patel20010@gmail.com", "mehul.chandroliya@gmail.com", "Selection Slip / ASR", "Test", null, null);
            //var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            //var users = customMembershipProvider.GetUsers();
            //var officeManager = new DataAccess.OfficeMananer();
            //var office = officeManager.GetOffices(0);
            return PartialView();
            //return View();
        }
    }
}