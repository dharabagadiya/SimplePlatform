using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class TasksController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Tasks", "tasks.js", ControllerName);

            return View();
        }

        public JsonResult GetTasks()
        {
            var taskManager = new DataModel.TaskManager();
            var isOfficeAdmin = UserDetail.User.Roles.Any(role => new List<int> { 1, 2 }.Contains(role.RoleId));
            List<DataModel.Modal.Task> taskList;
            if (IsAdmin)
            {
                taskList = taskManager.GetTasks().ToList();
            }
            else if (isOfficeAdmin)
            {
                var officeIDs = UserDetail.Offices.Select(model => model.OfficeId).ToList();
                taskList = taskManager.GetTasks().Where(model => officeIDs.Contains(model.Office.OfficeId)).ToList();
            }
            else
            {
                taskList = UserDetail.Tasks.ToList();
            }

            var tasks = taskList.Select(model => new
            {
                ID = model.TaskId,
                Title = model.Name,
                DueDate = model.EndDate.ToString("dd-MM-yyyy"),
                AssignTo = (model.UsersDetail == null ? model.Office.Name : (model.UsersDetail.UserId == UserDetail.UserId) ? "Me" : (model.UsersDetail.User.FirstName + " " + model.UsersDetail.User.LastName))
            }).ToList();
            return Json(new { data = tasks });
        }

        public ActionResult Add()
        {
            if (!IsAdmin) { return RedirectToAction("AddByOffice"); }
            var userDetailManager = new DataModel.UserManager();
            var officeMananer = new DataModel.OfficeMananer();
            var user = userDetailManager.GetUserDetail(UserDetail.UserId);
            var offices = IsAdmin ? officeMananer.GetOffices() : user.Offices.ToList();
            ViewData["Offices"] = offices;
            return PartialView();
        }

        public ActionResult AddByOffice()
        {
            var userDetailManager = new DataModel.UserManager();
            var officeMananer = new DataModel.OfficeMananer();
            var user = userDetailManager.GetUserDetail(UserDetail.UserId);
            var offices = user.Offices.ToList();
            ViewData["Offices"] = offices;
            ViewData["UserID"] = UserDetail.UserId;
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name, string startDate, string endDate, string description, int officeID, int userID)
        {
            var taskManager = new DataModel.TaskManager();
            var status = taskManager.Add(name, startDate, endDate, description, officeID, userID);
            return Json(status);
        }
    }
}