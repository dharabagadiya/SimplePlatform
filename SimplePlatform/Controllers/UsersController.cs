using DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace SimplePlatform.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Index()
        {
            StartupScript = "users.LoadUserPageSetting();";
            return View();
        }

        public JsonResult GetUsers()
        {
            var userManager = new DataModel.UserManager();
            var officesManager = new OfficeMananer();
            var offices = IsAdmin ? officesManager.GetOffices() : UserDetail.Offices.Where(model => model.IsDeleted == false).ToList();
            var userDetails = offices.SelectMany(model => model.UsersDetail).ToList();
            var users = userDetails.Where(model => (!model.User.Roles.Any(roleModel => roleModel.RoleId == 1) || IsAdmin) && model.User.IsDeleted == false)
                .Select(modal => new { id = modal.UserId, firstName = modal.User.FirstName, lastName = modal.User.LastName, createDate = modal.User.CreateDate.ToString("dd-MM-yyyy"), userRoles = string.Join(", ", modal.User.Roles.Select(roleModal => roleModal.RoleName).ToArray()), userRolesID = string.Join(", ", modal.User.Roles.Select(roleModal => roleModal.RoleId).ToArray()), userOfficesID = string.Join(", ", modal.Offices.Select(officeModel => officeModel.OfficeId).ToArray()) })
                .ToList();
            return Json(new { data = users });
        }

        public PartialViewResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var officeMananer = new DataModel.OfficeMananer();
            // Remove Admin from User Role
            var roles = IsAdmin ? customRoleProvider.GetAllRoles() : customRoleProvider.GetAllRoles().Where(model => model.RoleId != 1).ToList();
            ViewData["Offices"] = officeMananer.GetOffices();
            return PartialView(roles);
        }

        public PartialViewResult Edit(int id)
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var userManager = new DataModel.UserManager();
            var user = userManager.GetUserDetail(id);
            ViewData["UserRoles"] = customRoleProvider.GetAllRoles();
            var officeMananer = new DataModel.OfficeMananer();
            ViewData["Offices"] = officeMananer.GetOffices();
            return PartialView(user);
        }

        [HttpPost]
        public JsonResult Add(string firstName, string lastName, string emildID, int userRoleID, int officeID)
        {
            var userManager = new DataModel.UserManager();
            var status = userManager.CreateUser(firstName, lastName, emildID, userRoleID, officeID);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Update(int id, string firstName, string lastName, string emildID, int userRoleID, int officesID)
        {
            var userManager = new DataModel.UserManager();
            var status = userManager.UpdateUser(id, firstName, lastName, emildID, userRoleID, officesID);
            return Json(status);
        }

        [HttpPost]
        public JsonResult UpdateProfileImage()
        {
            //var userManager = new DataModel.UserManager();
            //var status = userManager.UpdateUser(id, firstName, lastName, emildID, userRoleID, officesID);
            //return Json(status);

            var status = false;
            HttpPostedFileBase myFile = null;
            if (Request.Files.Count > 0) myFile = Request.Files[0];
            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/ImageUploads");
                if (SharedFunction.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        string fileName = DateTime.Now.ToString("MMddyyyyHHmmss") + Path.GetExtension(myFile.FileName);
                        myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                        string path = "~/ImageUploads/" + fileName;
                        var userManager = new DataModel.UserManager();
                        status = userManager.UpdateUser(Convert.ToInt32(Request.Form["id"]), Request.Form["firstName"].ToString(), Request.Form["lastName"].ToString(), Request.Form["emildID"].ToString(), Convert.ToInt32(Request.Form["userRoleID"]), Convert.ToInt32(Request.Form["officesID"]), fileName, path);
                    }
                    catch (Exception ex)
                    {
                        return Json(ex.InnerException);
                    }
                }
            }
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