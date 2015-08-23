
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
            var dataSeries = new List<DataModel.Modal.ChartSeries>();
            var targetManager = new DataModel.TargetManager();
            var audienceManager = new DataModel.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices;
            var targets = targetManager.GetFundingTargets(offices.ToList(), DateTime.Now.Year);
            var achievedTargets = audienceManager.GetFundingTargetsAchived(offices.ToList(), DateTime.Now.Year);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }

        public JsonResult GetBookingTargets()
        {
            var dataSeries = new List<object>();
            var targetManager = new DataModel.TargetManager();
            var audienceManager = new DataModel.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices;
            var targets = targetManager.GetBookingTargets(offices.ToList(), DateTime.Now.Year);
            var achievedTargets = audienceManager.GetBookingTargetsAchived(offices.ToList(), DateTime.Now.Year);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }

        public JsonResult GetGSBTargets()
        {
            var dataSeries = new List<object>();
            var targetManager = new DataModel.TargetManager();
            var audienceManager = new DataModel.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices;
            var targets = targetManager.GetGSBTargets(offices.ToList(), DateTime.Now.Year);
            var achievedTargets = audienceManager.GetGSBTargetsAchived(offices.ToList(), DateTime.Now.Year);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }

        public JsonResult GetArrivalTargets()
        {
            var dataSeries = new List<object>();
            var targetManager = new DataModel.TargetManager();
            var audienceManager = new DataModel.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices;
            var targets = targetManager.GetArrivalTargets(offices.ToList(), DateTime.Now.Year);
            var achievedTargets = audienceManager.GetArrivalTargetsAchived(offices.ToList(), DateTime.Now.Year);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            var chartWidgetData = new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
            return Json(chartWidgetData);
        }
    }
}