using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class CalenderController : BaseController
    {
        // GET: Calender
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Calender", "Calender.js", ControllerName);

            BundleConfig.AddStyle("/Calender", "calender.css", ControllerName);

            return View();
        }
    }
}