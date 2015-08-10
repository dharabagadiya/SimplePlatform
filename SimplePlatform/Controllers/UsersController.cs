using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Users", "users.js", ControllerName);

            return View();
        }

        public JsonResult GetUsers()
        {
            var customMembershipProvide = new CustomAuthentication.CustomMembershipProvider();
            var users = customMembershipProvide.GetUsers().Select(modal => new { id = modal.UserId, firstName = modal.FirstName, lastName = modal.LastName, createDate = modal.CreateDate.ToString("dd-MM-yyyy"), userRoles = string.Join(", ", modal.Roles.Select(roleModal => roleModal.RoleName).ToArray()), userRolesID = string.Join(", ", modal.Roles.Select(roleModal => roleModal.RoleId).ToArray()) }).ToList();
            return Json(new { data = users });
        }

        public PartialViewResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            return PartialView(customRoleProvider.GetAllRoles());
        }

        public PartialViewResult Edit(int id)
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var userManager = new DataModel.UserManager();
            var user = userManager.GetUserDetail(id).User;
            ViewData["UserRoles"] = customRoleProvider.GetAllRoles();
            return PartialView(user);
        }

        [HttpPost]
        public JsonResult Add(string firstName, string lastName, string emildID, int userRoleID)
        {
            var userManager = new DataModel.UserManager();
            var status = userManager.CreateUser(firstName, lastName, emildID, userRoleID);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Update(int id, string firstName, string lastName, string emildID, int userRoleID)
        {
            var userManager = new DataModel.UserManager();
            var status = userManager.UpdateUser(id, firstName, lastName, emildID, userRoleID);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var userManager = new DataModel.UserManager();
            var status = userManager.DeleteUser(id);
            return Json(status);
        }
    }
}