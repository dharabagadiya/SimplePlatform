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
            var task = taskManager.GetTasks().Select(model => new { ID = model.TaskId, Title = model.Name, DueDate = model.EndDate.ToString("dd-MM-yyyy"), AssignTo = (model.UsersDetail == null ? "Me" : (model.UsersDetail.User.FirstName + " " + model.UsersDetail.User.LastName)) }).ToList();
            return Json(new { data = task });
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
            var offices = IsAdmin ? officeMananer.GetOffices() : user.Offices.ToList();
            ViewData["Offices"] = offices;
            if (IsAdmin)
            {
                var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
                var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
                var role = customRoleProvider.GetRole("Employee");
                ViewData["Employee"] = customMembershipProvider.GetUsers(role.RoleId);
            }
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