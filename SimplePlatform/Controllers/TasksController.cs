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
                taskList = taskManager.GetTasks().Where(model => model.UsersDetail == null).ToList();
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
                AssignTo = (model.UsersDetail == null ? model.Office.Name : (model.UsersDetail.UserId == UserDetail.UserId) ? "Me" : (model.UsersDetail.User.FirstName + " " + model.UsersDetail.User.LastName)),
                OfficeID = model.Office.OfficeId,
                UserID = (model.UsersDetail == null ? 0 : model.UsersDetail.UserId),
            }).ToList();
            return Json(new { data = tasks });
        }

        public ActionResult Add()
        {
            if (!IsAdmin) { return RedirectToAction("AddByOffice"); }
            var officeMananer = new DataModel.OfficeMananer();
            var offices = IsAdmin ? officeMananer.GetOffices() : UserDetail.Offices.ToList();
            ViewData["Offices"] = offices;
            return PartialView();
        }

        public ActionResult AddByOffice()
        {
            var offices = UserDetail.Offices.ToList();
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

        public ActionResult Edit(int id)
        {
            var officeMananer = new DataModel.OfficeMananer();
            var taskManager = new DataModel.TaskManager();
            var task = taskManager.GetTask(id);
            var offices = IsAdmin ? officeMananer.GetOffices() : UserDetail.Offices.ToList();
            ViewData["Offices"] = offices;
            return PartialView(task);
        }

        public JsonResult Update(int taskID, string name, string startDate, string endDate, string description, int officeID, int userID)
        {
            var taskManager = new DataModel.TaskManager();
            var status = taskManager.Update(taskID, name, startDate, endDate, description, officeID, userID);
            return Json(status);
        }

        public JsonResult Delete(int id)
        {
            var taskManager = new DataModel.TaskManager();
            var status = taskManager.Delete(id);
            return Json(status);
        }
    }
}