﻿
#region Using Namespaces
using CustomAuthentication.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

namespace SimplePlatform.Controllers
{
    [CustomAuthorize(Roles = "Admin, Offices, Employee")]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Home", "home.js", ControllerName);
            StartupScript = "home.DoPageSetting();";
            return View();
        }

        public JsonResult GetFundRaisingTargets()
        {
            var dataSeries = new List<object>();
            var targetManager = new DataModel.TargetManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices;
            dataSeries.Add(targetManager.GetFundingTargets(offices.ToList(), DateTime.Now.Year));
            return Json(dataSeries);
        }

        public JsonResult GetBookingTargets()
        {
            var dataSeries = new List<object>();
            var targetManager = new DataModel.TargetManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices;
            dataSeries.Add(targetManager.GetBookingTargets(offices.ToList(), DateTime.Now.Year));
            return Json(dataSeries);
        }
    }
}