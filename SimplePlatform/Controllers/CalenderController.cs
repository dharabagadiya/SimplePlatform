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

        public JsonResult GetEvents(string start, string end)
        {
            var startDate = Convert.ToDateTime(start);
            var endDate = Convert.ToDateTime(end);
            var taskManager = new DataModel.TaskManager();
            var isOfficeAdmin = UserDetail.User.Roles.Any(role => new List<int> { 1, 2 }.Contains(role.RoleId));
            List<DataModel.Modal.Task> taskList;

            if (IsAdmin)
            {
                taskList = taskManager.GetTasks(startDate, endDate).Where(model => model.UsersDetail == null).ToList();
            }
            else if (isOfficeAdmin)
            {
                var officeIDs = UserDetail.Offices.Select(model => model.OfficeId).ToList();
                taskList = taskManager.GetTasks(startDate, endDate).Where(model => officeIDs.Contains(model.Office.OfficeId)).ToList();
            }
            else
            {
                taskList = UserDetail.Tasks.Where(model => model.IsDeleted == false && (model.StartDate >= startDate && model.StartDate <= endDate)).ToList();
            }

            var tasks = taskList.Select(model => new
            {
                id = model.TaskId,
                title = model.Name,
                start = model.StartDate.ToString("yyyy-MM-dd"),
                end = model.EndDate.ToString("yyyy-MM-dd")
            }).ToList();

            return Json(tasks);
        }
    }
}