using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using System.Web.Script.Serialization;
using System.IO;
using Utilities;

namespace SimplePlatform.Controllers
{
    public class OfficesController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddStyle("/Offices", "Offices.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Offices.js", ControllerName);
            Script = string.Format("office.options.isEditDeleteEnable = {0};", IsAdmin.ToString().ToLower());
            return View();
        }

        public object GetFundRaisingTargets(int id, DateTime startDate, DateTime endDate)
        {
            var targetManager = new TargetManager();
            var audienceManager = new AudienceManager();
            var offices = new OfficeMananer().GetOffice(id);
            var targets = targetManager.GetFundingTargets(new List<DataModel.Modal.Office> { offices }, startDate, endDate);
            var achievedTargets = audienceManager.GetFundingTargetsAchived(new List<DataModel.Modal.Office> { offices }, startDate, endDate);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            return new { Total = totalTargets, ActTotal = totalAchievedTargets };
        }

        public object GetTaskTargets(int id, DateTime startDate, DateTime endDate)
        {
            var taskManager = new TaskManager();
            var tasks = taskManager.GetTasks(id, startDate, endDate);
            var totalTask = tasks.Count();
            var totalTaskCompleted = tasks.Where(model => model.IsCompleted == true && model.UsersDetail == null).Count();
            return new { Total = totalTask, ActTotal = totalTaskCompleted };
        }

        public object GetBookingTargets(int id, DateTime startDate, DateTime endDate)
        {
            var dataSeries = new List<object>();
            var targetManager = new TargetManager();
            var audienceManager = new AudienceManager();
            var offices = new OfficeMananer().GetOffice(id);
            var targets = targetManager.GetBookingTargets(new List<DataModel.Modal.Office> { offices }, startDate, endDate);
            var achievedTargets = audienceManager.GetBookingTargetsAchived(new List<DataModel.Modal.Office> { offices }, startDate, endDate);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Count();
            return new { Total = totalTargets, ActTotal = totalAchievedTargets };
        }

        public object GetArrivalTargets(int id, DateTime startDate, DateTime endDate)
        {
            var targetManager = new TargetManager();
            var audienceManager = new AudienceManager();
            var offices = new OfficeMananer().GetOffice(id);
            var targets = targetManager.GetArrivalTargets(new List<DataModel.Modal.Office> { offices }, startDate, endDate);
            var achievedTargets = audienceManager.GetArrivalTargetsAchived(new List<DataModel.Modal.Office> { offices }, startDate, endDate);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            return new { Total = totalTargets, ActTotal = totalAchievedTargets };
        }

        [HttpPost]
        public JsonResult GetOffices(int pageNo, int pageSize, string startDate, string endDate)
        {
            var officesManager = new OfficeMananer();
            var offices = IsAdmin ? officesManager.GetOffices() : UserDetail.Offices;
            var totalRecord = offices.Count();
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var filteredOffices = offices
                .Where(model => model.IsDeleted == false)
                .OrderBy(model => model.Name)
                .Select(modal => new
                {
                    ID = modal.OfficeId,
                    Name = modal.Name,
                    Fundraising = GetFundRaisingTargets(modal.OfficeId, startDateTime, endDateTime),
                    Task = GetTaskTargets(modal.OfficeId, startDateTime, endDateTime),
                    Arrival = GetBookingTargets(modal.OfficeId, startDateTime, endDateTime),
                    BookingInProcess = GetArrivalTargets(modal.OfficeId, startDateTime, endDateTime),
                    ProfilePic = modal.FileResource == null ? "" : Url.Content(modal.FileResource.path)
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
            if (!IsAdmin) { return Json(false); }
            var officesManager = new OfficeMananer();
            return Json(officesManager.Add(name, contactNo, city, userID));
        }

        [HttpPost]
        public JsonResult Update(int id, string name, string contactNo, string city, int userID)
        {
            if (!IsAdmin) { return Json(false); }
            var officesManager = new OfficeMananer();
            return Json(officesManager.Update(id, name, contactNo, city, userID));
        }
        [HttpPost]
        public JsonResult UpdateFile()
        {
            var status = false;
            HttpPostedFileBase myFile = null;
            if (Request.Files.Count > 0) myFile = Request.Files[0];
            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/ImageUploads");
                if (SharedFunction.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        string fileName = DateTime.Now.ToString("MMddyyyyHHmmss") + Path.GetExtension(myFile.FileName);
                        myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                        string path = "~/ImageUploads/" + fileName;
                        var officesManager = new OfficeMananer();
                        status = officesManager.Update(Convert.ToInt32(Request.Form["id"]), Request.Form["name"].ToString(), Request.Form["contactNo"].ToString(), Request.Form["city"].ToString(), Convert.ToInt32(Request.Form["userID"]), path);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return Json(status);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (!IsAdmin) { return Json(false); }
            var officesManager = new OfficeMananer();
            return Json(officesManager.Delete(id));
        }

        // Fund Raising Target For CurrentYear
        public object GetFundRaisingTargetsChart(int id, DateTime startDate, DateTime endDate)
        {
            var dataSeries = new List<DataModel.Modal.ChartSeries>();
            var targetManager = new TargetManager();
            var audienceManager = new AudienceManager();
            var office = new OfficeMananer().GetOffice(id);
            var targets = targetManager.GetFundingTargets(new List<DataModel.Modal.Office> { office }, startDate, endDate);
            var achievedTargets = audienceManager.GetFundingTargetsAchived(new List<DataModel.Modal.Office> { office }, startDate, endDate);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            return new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
        }

        public object GetTaskForCurrentWeek(int id, DateTime startDate, DateTime endDate)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = Utilities.DateTimeUtilities.GetIso8601WeekOfYear(DateTime.Now);
            var taskManager = new TaskManager();
            var tasks = taskManager.GetTasks(id, startDate, endDate);
            return tasks.Select(model => new
            {
                ID = model.TaskId,
                Name = model.Name,
                EndDate = model.EndDate.ToString("MM dd,yyyy"),
                Description = model.Description,
                AssignTo = model.UsersDetail == null ? "Me" : (model.UsersDetail.User.FirstName + " " + model.UsersDetail.User.LastName),
                IsCompleted = model.IsCompleted
            }).ToList();
        }

        public object GetUserArrivalForCurrentWeek(int id, DateTime startDate, DateTime endDate)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = Utilities.DateTimeUtilities.GetIso8601WeekOfYear(DateTime.Now);
            var audienceManager = new AudienceManager();
            var audiences = audienceManager.GetArrivalAudiences(id, startDate, endDate);
            return audiences.Select(model => new
            {
                ID = model.AudienceID,
                Name = model.Name,
                ConventionName = model.Convention.Name,
                ArrivalDate = model.Convention.StartDate.ToString("MM dd,yyyy"),
                IsAttended = model.IsAttended
            });
        }

        public ActionResult Detail(int id)
        {
            var offices = new OfficeMananer().GetOffice(id);
            BundleConfig.AddStyle("/Offices", "Detail.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Detail.js", ControllerName);
            Script = string.Format("officeDetail.options.officeID = {0};", id);
            return View(offices);
        }

        [HttpPost]
        public JsonResult Detail(int id, string startDate, string endDate)
        {
            var offices = new OfficeMananer().GetOffice(id);
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var fundRaisingTargetData = GetFundRaisingTargetsChart(id, startDateTime, endDateTime);
            var bookingTargetData = GetBookingTargets(id, startDateTime, endDateTime);
            var tasks = GetTaskForCurrentWeek(id, startDateTime, endDateTime);
            var arrivalAudiences = GetUserArrivalForCurrentWeek(id, startDateTime, endDateTime);
            return Json(new { fundRaisingTargetData = fundRaisingTargetData, bookingTargetData = bookingTargetData, tasks = tasks, arrivalAudiences = arrivalAudiences });
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            var status = false;
            HttpPostedFileBase myFile = null;
            if (Request.Files.Count > 0) myFile = Request.Files[0];
            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/ImageUploads");
                if (SharedFunction.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        string fileName = DateTime.Now.ToString("MMddyyyyHHmmss") + Path.GetExtension(myFile.FileName);
                        myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                        string path = "~/ImageUploads/" + fileName;
                        var officesManager = new OfficeMananer();
                        status = officesManager.Add(Request.Form["name"].ToString(), Request.Form["contactNo"].ToString(), Request.Form["city"].ToString(), Convert.ToInt32(Request.Form["userID"]), path, myFile.FileName);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return Json(status);
        }
    }
}