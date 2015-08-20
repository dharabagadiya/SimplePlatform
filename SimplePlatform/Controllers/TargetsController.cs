
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
            BundleConfig.AddScript("~/Scripts/Offices", "targets.js", ControllerName);
            return View();
        }

        public JsonResult GetTasks()
        {
            var targetManager = new DataModel.TargetManager();
            var targetList = targetManager.GetTargets();
            var targets = targetList.Select(model => new
            {
                ID = model.TargetId,
                OfficeName = model.Office.Name,
                DueDate = model.DueDate,
                Booking = model.Booking,
                FundRaising = model.FundRaising,
                GSB = model.GSB,
                Arrivals = model.Arrivals
            }).ToList();
            return Json(new { data = targets });
        }
    }
}