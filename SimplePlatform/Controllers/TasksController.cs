using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class TasksController : BaseController
    {
        public ActionResult Index(int id = 0)
        {
            return View();
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
            taskManager.Add(name, startDate, endDate, description, officeID, userID);
            return Json(null);
        }
    }
}