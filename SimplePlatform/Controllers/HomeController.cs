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

        public JsonResult GetFundRaisingTargets(string startDate, string endDate)
        {
            var dataSeries = new List<DataModel.Modal.ChartSeries>();
            var targetManager = new DataAccess.TargetManager();
            var audienceManager = new DataAccess.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices.Where(model => model.IsDeleted == false).ToList();
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var targets = targetManager.GetFundingTargets(offices.Select(model => model.OfficeId).ToList(), startDateTime, endDateTime);
            var achievedTargets = audienceManager.GetFundingTargetsAchived(offices.Select(model => model.OfficeId).ToList(), startDateTime, endDateTime);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }

        public JsonResult GetBookingTargets(string startDate, string endDate)
        {
            var dataSeries = new List<object>();
            var targetManager = new DataAccess.TargetManager();
            var audienceManager = new DataAccess.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices.Where(model => model.IsDeleted == false).ToList();
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var targets = targetManager.GetBookingTargets(offices.Select(model => model.OfficeId).ToList(), startDateTime, endDateTime);
            var achievedTargets = audienceManager.GetBookingTargetsAchived(offices.Select(model => model.OfficeId).ToList(), startDateTime, endDateTime);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }

        public JsonResult GetGSBTargets(string startDate, string endDate)
        {
            var dataSeries = new List<object>();
            var targetManager = new DataAccess.TargetManager();
            var audienceManager = new DataAccess.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices.Where(model => model.IsDeleted == false).ToList();
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var targets = targetManager.GetGSBTargets(offices.Select(model => model.OfficeId).ToList(), startDateTime, endDateTime);
            var achievedTargets = audienceManager.GetGSBTargetsAchived(offices.Select(model => model.OfficeId).ToList(), startDateTime, endDateTime);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }

        public JsonResult GetArrivalTargets(string startDate, string endDate)
        {
            var dataSeries = new List<object>();
            var targetManager = new DataAccess.TargetManager();
            var audienceManager = new DataAccess.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices.Where(model => model.IsDeleted == false).ToList();
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var targets = targetManager.GetArrivalTargets(offices.Select(model => model.OfficeId).ToList(), startDateTime, endDateTime);
            var achievedTargets = audienceManager.GetArrivalTargetsAchived(offices.Select(model => model.OfficeId).ToList(), startDateTime, endDateTime);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }
    }
}