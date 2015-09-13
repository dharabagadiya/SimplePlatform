using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;

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

        public PartialViewResult EventDetail(int id)
        {
            var eventManager = new DataModel.EventManager();
            var eventDetail = eventManager.GetEventDetail(id);
            return PartialView(eventDetail);
        }

        public PartialViewResult TaskDetail(int id)
        {
            var taskManager= new DataModel.TaskManager();
            var taskDetail = taskManager.GetTask(id);
            return PartialView(taskDetail);
        }

        private List<dynamic> GetTasks(DateTime startDate, DateTime endDate)
        {
            var taskManager = new DataModel.TaskManager();
            var isOfficeAdmin = UserDetail.User.Roles.Any(role => new List<int> { 1, 2 }.Contains(role.RoleId));
            List<DataModel.Modal.Task> taskList;

            if (IsAdmin)
            {
                taskList = taskManager.GetTasks(startDate, endDate).Where(model => model.UsersDetail == null).ToList();
            }
            else if (isOfficeAdmin)
            {
                var officeIDs = UserDetail.Offices.Where(model => model.IsDeleted == false).Select(model => model.OfficeId).ToList();
                taskList = taskManager.GetTasks(startDate, endDate).Where(model => officeIDs.Contains(model.Office.OfficeId)).ToList();
            }
            else
            {
                taskList = UserDetail.Tasks.Where(model => model.IsDeleted == false && (model.StartDate >= startDate && model.StartDate <= endDate)).ToList();
            }
            return taskList.Select(model => new
            {
                type = "TASK",
                id = model.TaskId,
                className = "Calender-Event",
                title = model.Name,
                start = model.StartDate.ToString("yyyy-MM-ddThh:mm:ss"),
                end = model.EndDate.ToString("yyyy-MM-ddThh:mm:ss"),
                imageURL = model.UsersDetail == null ? (model.Office == null ? "Content/Images/Common/avatar.png" : Url.Content(model.Office.FileResource.path)) : Url.Content(model.UsersDetail.FileResource.path)
            }).ToList<dynamic>();
        }
        private List<dynamic> GetEvents(DateTime startDate, DateTime endDate)
        {
            var eventManager = new EventManager();
            var officeManager = new OfficeMananer();
            var offices = IsAdmin ? officeManager.GetOffices() : officeManager.GetOffices(UserDetail.UserId);
            var events = eventManager.GetEvents(offices.Select(model => model.OfficeId).ToList());
            var eventList = events.Where(model => model.IsDeleted == false && (model.StartDate >= startDate && model.StartDate <= endDate)).ToList();
            return eventList.Select(model => new
            {
                type = "EVENT",
                id = model.EventId,
                title = model.Name,
                start = model.StartDate.ToString("yyyy-MM-ddThh:mm:ss"),
                end = model.EndDate.ToString("yyyy-MM-ddThh:mm:ss"),
                imageURL = (model.Office == null ? "Content/Images/Common/avatar.png" : Url.Content(model.Office.FileResource.path))
            }).ToList<dynamic>();
        }

        public JsonResult GetEvents(string start, string end)
        {
            var startDate = Convert.ToDateTime(start);
            var endDate = Convert.ToDateTime(end);
            var events = new List<List<dynamic>>();
            events.Add(GetTasks(startDate, endDate));
            events.Add(GetEvents(startDate, endDate));
            return Json(events.SelectMany(model => model).ToList());
        }
    }
}