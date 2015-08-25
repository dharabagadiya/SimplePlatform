﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using System.Web.Script.Serialization;

namespace SimplePlatform.Controllers
{
    public class OfficesController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddStyle("/Offices", "Offices.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Offices.js", ControllerName);
            return View();
        }

        [HttpPost]
        public JsonResult GetOffices(int pageNo = 1, int pageSize = 3)
        {
            var officesManager = new OfficeMananer();
            var offices = officesManager.GetOffices();
            var totalRecord = offices.Count();
            var filteredOffices = offices.Select(modal => new
            {
                ID = modal.OfficeId,
                Name = modal.Name,
                Fundraising = new { ActTotal = 5, Total = 10 },
                Task = new { ActTotal = 5, Total = 100 },
                Events = new { ActTotal = 5, Total = 100 },
                BookingInProcess = new { ActTotal = 5, Total = 100 },
                BookingConfirm = new { ActTotal = 5, Total = 100 }
            }).OrderBy(modal => modal.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                totalRecord = totalRecord,
                currentPage = pageNo,
                pageSize = pageSize,
                offices = filteredOffices
            });
        }

        public PartialViewResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var roleID = customRoleProvider.GetRole("Offices");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var Users = customMembershipProvider.GetUsers(roleID.RoleId);
            return PartialView(Users);
        }

        public PartialViewResult Edit(int id)
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var roleID = customRoleProvider.GetRole("Offices");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var Users = customMembershipProvider.GetUsers(roleID.RoleId);
            ViewData["Users"] = Users;
            var officesManager = new OfficeMananer();
            return PartialView(officesManager.GetOffice(id));
        }

        [HttpPost]
        public JsonResult Add(string name, string contactNo, string city, int userID)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Add(name, contactNo, city, userID));
        }

        [HttpPost]
        public JsonResult Update(int id, string name, string contactNo, string city, int userID)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Update(id, name, contactNo, city, userID));
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Delete(id));
        }

        // Fund Raising Target For CurrentYear
        public object GetFundRaisingTargets(int id)
        {
            var dataSeries = new List<DataModel.Modal.ChartSeries>();
            var targetManager = new DataModel.TargetManager();
            var audienceManager = new DataModel.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices;
            var currentYear = DateTime.Now.Year;
            var targets = targetManager.GetFundingTargets(offices.ToList(), currentYear);
            var achievedTargets = audienceManager.GetFundingTargetsAchived(offices.ToList(), currentYear);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            return new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
        }

        public object GetBookingTargets(int id)
        {
            var dataSeries = new List<object>();
            var targetManager = new DataModel.TargetManager();
            var audienceManager = new DataModel.AudienceManager();
            var offices = IsAdmin ? new DataModel.OfficeMananer().GetOffices() : UserDetail.Offices;
            var currentYear = DateTime.Now.Year;
            var currentWeek = Utilities.DateTimeUtilities.GetIso8601WeekOfYear(DateTime.Now);
            var targets = targetManager.GetBookingTargets(offices.ToList(), currentYear);
            var achievedTargets = audienceManager.GetBookingTargetsAchived(offices.ToList(), currentYear);
            var totalTargets = targets.data.Where(model => model.weekNumber == currentWeek).Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Where(model => model.weekNumber == currentWeek).Sum(model => model.y);
            return new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets };
        }

        public object GetTaskForCurrentWeek(int id)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = Utilities.DateTimeUtilities.GetIso8601WeekOfYear(DateTime.Now);
            var taskManager = new TaskManager();
            var tasks = taskManager.GetTasks(id, currentYear, currentWeek);
            return tasks.Select(model => new
            {
                ID = model.TaskId,
                Name = model.Name,
                EndDate = model.EndDate.ToString("MM dd,yyyy"),
                Description = model.Description
            }).ToList();
        }

        public object GetUserArrivalForCurrentWeek(int id)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = Utilities.DateTimeUtilities.GetIso8601WeekOfYear(DateTime.Now);
            var audienceManager = new AudienceManager();
            var audiences = audienceManager.GetArrivalAudiences(currentYear, currentWeek);
            return audiences.Select(model => new
            {
                ID = model.AudienceID,
                Name = model.Name,
                ConventionName  = model.Convention.Name,
                ArrivalDate = model.Convention.StartDate.ToString("MM dd,yyyy")
            });
        }

        public ActionResult Detail(int id)
        {

            BundleConfig.AddStyle("/Offices", "Detail.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Detail.js", ControllerName);

            Script = string.Format("var fundRaisingTargetData = {0};", new JavaScriptSerializer().Serialize(GetFundRaisingTargets(id)));
            Script = string.Format("var bookingTargetData = {0};", new JavaScriptSerializer().Serialize(GetBookingTargets(id)));
            Script = string.Format("var tasks = {0};", new JavaScriptSerializer().Serialize(GetTaskForCurrentWeek(id)));
            Script = string.Format("var arrivalAudiences = {0};", new JavaScriptSerializer().Serialize(GetUserArrivalForCurrentWeek(id)));
            return View();
        }
    }
}