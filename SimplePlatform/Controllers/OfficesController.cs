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
            var officeMananer = new DataAccess.OfficeMananer();
            var offices = officeMananer.GetOffices(UserDetail.UserId);
            var totalOffices = offices.Count();
            var office = offices.FirstOrDefault();
            if (totalOffices == 1) { return RedirectToAction("Detail", new { id = office.OfficeId }); }

            BundleConfig.AddStyle("/Offices", "Offices.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Offices.js", ControllerName);
            Script = string.Format("office.options.isEditDeleteEnable = {0};", IsAdmin.ToString().ToLower());
            return View();
        }

        public object GetFundRaisingTargets(int id, DateTime startDate, DateTime endDate)
        {
            var targetManager = new DataAccess.TargetManager();
            var audienceManager = new DataAccess.AudienceManager();
            var targets = targetManager.GetFundingTargets(new List<int> { id }, startDate, endDate);
            var achievedTargets = audienceManager.GetFundingTargetsAchived(new List<int> { id }, startDate, endDate);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            return new { Total = totalTargets, ActTotal = totalAchievedTargets };
        }

        public object GetTaskTargets(int id, DateTime startDate, DateTime endDate)
        {
            var taskManager = new DataAccess.TaskManager();
            var tasks = taskManager.GetTasks(startDate, endDate, id, 0);
            var totalTask = tasks.Count();
            var totalTaskCompleted = tasks.Where(model => model.IsCompleted == true && model.UsersDetail == null).Count();
            return new { Total = totalTask, ActTotal = totalTaskCompleted };
        }

        public object GetBookingTargets(int id, DateTime startDate, DateTime endDate)
        {
            var dataSeries = new List<object>();
            var targetManager = new DataAccess.TargetManager();
            var audienceManager = new DataAccess.AudienceManager();
            var targets = targetManager.GetBookingTargets(new List<int> { id }, startDate, endDate);
            var achievedTargets = audienceManager.GetBookingTargetsAchived(new List<int> { id }, startDate, endDate);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            return new { Total = totalTargets, ActTotal = totalAchievedTargets };
        }

        public object GetArrivalTargets(int id, DateTime startDate, DateTime endDate)
        {
            var targetManager = new DataAccess.TargetManager();
            var audienceManager = new DataAccess.AudienceManager();
            var targets = targetManager.GetArrivalTargets(new List<int> { id }, startDate, endDate);
            var achievedTargets = audienceManager.GetArrivalTargetsAchived(new List<int> { id }, startDate, endDate);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            return new { Total = totalTargets, ActTotal = totalAchievedTargets };
        }

        public object GetEventsTarget(int id, DateTime startDate, DateTime endDate)
        {
            var events = new DataAccess.EventManager().GetEvents(new List<int> { id }, startDate, endDate);
            var totalTargets = events.Count();
            return new { Total = totalTargets, ActTotal = totalTargets };
        }

        [HttpPost]
        public JsonResult GetOffices(int pageNo, int pageSize, string startDate, string endDate)
        {
            try
            {
                var officesManager = new DataAccess.OfficeMananer();
                var offices = officesManager.GetOffices(IsAdmin ? 0 : UserDetail.UserId);
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
                        Arrival = GetArrivalTargets(modal.OfficeId, startDateTime, endDateTime),
                        BookingInProcess = GetBookingTargets(modal.OfficeId, startDateTime, endDateTime),
                        Events = GetEventsTarget(modal.OfficeId, startDateTime, endDateTime),
                        ProfilePic = modal.FileResource == null ? Url.Content("~/Content/Images/Common/office_convention_avatar.png") : Url.Content(modal.FileResource.path)
                    }).OrderBy(modal => modal.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    totalRecord = totalRecord,
                    currentPage = pageNo,
                    pageSize = pageSize,
                    offices = filteredOffices
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public PartialViewResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var roleID = customRoleProvider.GetRole("Office");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var Users = customMembershipProvider.GetUsers(roleID.RoleId);
            return PartialView(Users);
        }

        public PartialViewResult Edit(int id)
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var roleID = customRoleProvider.GetRole("Office");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var Users = customMembershipProvider.GetUsers(roleID.RoleId);
            ViewData["Users"] = Users;
            var officesManager = new DataAccess.OfficeMananer();
            return PartialView(officesManager.GetOffice(id));
        }

        [HttpPost]
        public JsonResult Add(string name, string contactNo, string city, List<string> userID)
        {
            if (!IsAdmin) { return Json(false); }
            var officesManager = new DataAccess.OfficeMananer();
            return Json(officesManager.Add(name, contactNo, city, userID));
        }

        [HttpPost]
        public JsonResult Update(int id, string name, string contactNo, string city, List<string> userID)
        {
            if (!IsAdmin) { return Json(false); }
            var officesManager = new DataAccess.OfficeMananer();
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
                        var officesManager = new DataAccess.OfficeMananer();
                        status = officesManager.Update(Convert.ToInt32(Request.Form["id"]), Request.Form["name"].ToString(), Request.Form["contactNo"].ToString(), Request.Form["city"].ToString(), Request.Form["userID"].Split(',').ToList(), path, myFile.FileName);
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
            var officesManager = new DataAccess.OfficeMananer();
            return Json(officesManager.Delete(id));
        }

        // Fund Raising Target For CurrentYear
        public object GetFundRaisingTargetsChart(int id, DateTime startDate, DateTime endDate)
        {
            var dataSeries = new List<DataModel.Modal.ChartSeries>();
            var targetManager = new DataAccess.TargetManager();
            var audienceManager = new DataAccess.AudienceManager();
            var targets = targetManager.GetFundingTargets(new List<int> { id }, startDate, endDate);
            var achievedTargets = audienceManager.GetFundingTargetsAchived(new List<int> { id }, startDate, endDate);
            dataSeries.Add(targets);
            dataSeries.Add(achievedTargets);
            var totalTargets = targets.data.Sum(model => model.y);
            var totalAchievedTargets = achievedTargets.data.Sum(model => model.y);
            return new { TotalTarget = totalTargets, TotalTargetAchieved = totalAchievedTargets, AchivedTarget = 0, ChartData = dataSeries };
        }

        public object GetTaskForCurrentWeek(int id, DateTime startDate, DateTime endDate)
        {
            var isUpdateEnable = false;
            var isOfficeAdmin = UserDetail.User.Roles.Any(role => new List<int> { 1, 2 }.Contains(role.RoleId));
            var currentYear = DateTime.Now.Year;
            var currentWeek = Utilities.DateTimeUtilities.GetIso8601WeekOfYear(DateTime.Now);
            var taskManager = new DataAccess.TaskManager();
            List<DataModel.Modal.Task> taskList;
            if (IsAdmin)
            {
                taskList = taskManager.GetTasks(startDate, endDate, id, 0).Where(model => model.UsersDetail == null).ToList();
                isUpdateEnable = true;
            }
            else if (isOfficeAdmin)
            {
                taskList = taskManager.GetTasks(startDate, endDate, id, 0).ToList();
            }
            else
            {
                taskList = taskManager.GetTasks(startDate, endDate, id, UserDetail.UserId).ToList();
            }
            return taskList.Select(model => new
            {
                ID = model.TaskId,
                Name = model.Name,
                EndDate = model.EndDate.ToString("MM dd,yyyy"),
                Description = model.Description,
                AssignTo = model.UsersDetail == null ? "Me" : (model.UsersDetail.User.FirstName + " " + model.UsersDetail.User.LastName),
                IsCompleted = model.IsCompleted,
                IsUpdateEnable = isUpdateEnable ? isUpdateEnable : (model.UsersDetail != null && isOfficeAdmin ? true : false)
            }).ToList();
        }

        public object GetUserArrivalForCurrentWeek(int id, DateTime startDate, DateTime endDate)
        {
            var audienceManager = new DataAccess.AudienceManager();
            var audiences = audienceManager.GetArrivalAudiences(id, startDate, endDate);
            if (audiences == null) { return null; }
            return audiences.Select(model => new
            {
                ID = model.AudienceID,
                Name = model.Name,
                ConventionName = model.Convention.Name,
                ArrivalDate = model.Convention.StartDate.ToString("MM dd,yyyy"),
                IsAttended = model.IsAttended
            });
        }

        public object GetEventsForCurrentWeek(int id, DateTime startDate, DateTime endDate)
        {
            var eventManager = new DataAccess.EventManager();
            var events = eventManager.GetEvents(new List<int> { id }, startDate, endDate);
            return events.OrderBy(model => model.StartDate - DateTime.Now)
                .Select(model => new
                {
                    ID = model.EventId,
                    Name = model.Name,
                    StartDate = model.StartDate.ToString("MM dd,yyyy"),
                    EndDate = model.EndDate.ToString("MM dd,yyyy"),
                    TotalPeopleAttended = model.TotalAttended
                });
        }

        public ActionResult Detail(int id)
        {
            var offices = new DataAccess.OfficeMananer().GetOffice(id);
            BundleConfig.AddStyle("/Offices", "Detail.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Detail.js", ControllerName);
            Script = string.Format("officeDetail.options.officeID = {0};", id);
            StartupScript = "officeDetail.DoPageSetting();";
            return View(offices);
        }

        [HttpPost]
        public JsonResult Detail(int id, string startDate, string endDate)
        {
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var fundRaisingTargetData = GetFundRaisingTargetsChart(id, startDateTime, endDateTime);
            var bookingTargetData = GetBookingTargets(id, startDateTime, endDateTime);
            var tasks = GetTaskForCurrentWeek(id, startDateTime, endDateTime);
            var arrivalAudiences = GetUserArrivalForCurrentWeek(id, startDateTime, endDateTime);
            var events = GetEventsForCurrentWeek(id, startDateTime, endDateTime);
            return Json(new { fundRaisingTargetData = fundRaisingTargetData, bookingTargetData = bookingTargetData, tasks = tasks, arrivalAudiences = arrivalAudiences, events = events });
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
                        var officesManager = new DataAccess.OfficeMananer();
                        status = officesManager.Add(Request.Form["name"].ToString(), Request.Form["contactNo"].ToString(), Request.Form["city"].ToString(), Request.Form["userID"].Split(',').ToList(), path, myFile.FileName);
                    }
                    catch (Exception ex)
                    {
                        return Json(ex.InnerException);
                    }
                }
            }
            return Json(status);
        }
    }
}