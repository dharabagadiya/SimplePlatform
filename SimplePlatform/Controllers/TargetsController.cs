
#region Using Regions
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

namespace SimplePlatform.Controllers
{
    public class TargetsController : BaseController
    {
        // GET: Targets
        public ActionResult Index()
        {
            var officeManager = new DataModel.OfficeMananer();
            ViewData["Offices"] = officeManager.GetOffices();

            return View();
        }
    }
}