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
            var userManager = new DataModel.UserManager();
            var user = userManager.GetUser(UserDetail.UserId);
            var offices = user.Offices.ToList();
            ViewData["Offices"] = offices;
            if (IsAdmin)
            {
                var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
                var role = customRoleProvider.GetRole("Employee");
                ViewData["Employee"] = userManager.GetUsers(role.RoleId);
            }
            return PartialView();
        }
    }
}