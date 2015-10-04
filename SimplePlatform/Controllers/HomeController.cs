
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
    [CustomAuthorize(Roles = "Admin, Office, Office Staff")]
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
            var offices = new DataAccess.OfficeMananer().GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var targets = targetManager.GetFundingTargets(offices, startDateTime, endDateTime);
            var achievedTargets = audienceManager.GetFundingTargetsAchived(offices.ToList(), startDateTime, endDateTime);
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
            var offices = new DataAccess.OfficeMananer().GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var targets = targetManager.GetBookingTargets(offices, startDateTime, endDateTime);
            var achievedTargets = audienceManager.GetBookingTargetsAchived(offices, startDateTime, endDateTime);
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
            var offices = new DataAccess.OfficeMananer().GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var targets = targetManager.GetGSBTargets(offices, startDateTime, endDateTime);
            var achievedTargets = audienceManager.GetGSBTargetsAchived(offices, startDateTime, endDateTime);
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
            var offices = new DataAccess.OfficeMananer().GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var targets = targetManager.GetArrivalTargets(offices, startDateTime, endDateTime);
            var achievedTargets = audienceManager.GetArrivalTargetsAchived(offices, startDateTime, endDateTime);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }
    }
}