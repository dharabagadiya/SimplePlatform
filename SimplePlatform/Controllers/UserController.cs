using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            return PartialView(customRoleProvider.GetAllRoles());
        }

        [HttpPost]
        public JsonResult Add(string firstName, string lastName, string emildID, int userRoleID)
        {
            var customMembershipProvide = new CustomAuthentication.CustomMembershipProvider();
            var status = customMembershipProvide.CreateUser(firstName, lastName, emildID, userRoleID);
            return Json(status);
        }
    }
}