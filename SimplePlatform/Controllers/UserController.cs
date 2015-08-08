using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class UserController : BaseController
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
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var user = customMembershipProvider.GetUsers().Where(modal => modal.UserId == id).FirstOrDefault();
            ViewData["UserRoles"] = customRoleProvider.GetAllRoles();
            return PartialView(user);
        }

        [HttpPost]
        public JsonResult Add(string firstName, string lastName, string emildID, int userRoleID)
        {
            var customMembershipProvide = new CustomAuthentication.CustomMembershipProvider();
            var status = customMembershipProvide.CreateUser(firstName, lastName, emildID, userRoleID);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Update(int id, string firstName, string lastName, string emildID, int userRoleID)
        {
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var status = customMembershipProvider.UpdateUser(id, firstName, lastName, emildID, userRoleID);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var status = customMembershipProvider.DeleteUser(id);
            return Json(status);
        }
    }
}