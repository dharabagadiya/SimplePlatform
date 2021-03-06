﻿using DataModel;
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
            var userManager = new DataAccess.UserManager();
            var userDetails = userManager.GetUsers(IsAdmin ? 0 : UserDetail.UserId);
            var users = userDetails.Where(model => (!model.User.Roles.Any(roleModel => roleModel.RoleId == 1) || IsAdmin))
                .Select(modal => new { id = modal.UserId, firstName = modal.User.FirstName, lastName = modal.User.LastName, createDate = modal.User.CreateDate.ToString("dd-MM-yyyy"), userRoles = string.Join(", ", modal.User.Roles.Select(roleModal => roleModal.RoleName).ToArray()), userRolesID = string.Join(",", modal.User.Roles.Select(roleModal => roleModal.RoleId).ToArray()), userOfficesID = string.Join(",", modal.Offices.Select(officeModel => officeModel.OfficeId).ToArray()) })
                .ToList();
            return Json(new { data = users });
        }

        public PartialViewResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var officeMananer = new DataAccess.OfficeMananer();
            // Remove Admin from User Role
            var roles = IsAdmin ? customRoleProvider.GetAllRoles() : customRoleProvider.GetAllRoles().Where(model => model.RoleId != 1 && model.RoleId != 2).ToList();
            ViewData["Offices"] = officeMananer.GetOffices(IsAdmin ? 0 : UserDetail.UserId);
            return PartialView(roles);
        }

        public PartialViewResult Edit(int id)
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var userManager = new DataAccess.UserManager();
            var user = userManager.GetUserDetail(id);
            var roles = IsAdmin ? customRoleProvider.GetAllRoles() : customRoleProvider.GetAllRoles().Where(model => model.RoleId != 1 && model.RoleId != 2).ToList();
            ViewData["UserRoles"] = roles;
            var officeMananer = new DataAccess.OfficeMananer();
            ViewData["Offices"] = officeMananer.GetOffices(IsAdmin ? 0 : UserDetail.UserId);
            return PartialView(user);
        }

        [HttpPost]
        public JsonResult Add(string firstName, string lastName, string emildID, int userRoleID, List<string> officeID)
        {
            var userManager = new DataAccess.UserManager();
            var status = userManager.CreateUser(firstName, lastName, emildID, userRoleID, officeID);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Update(int id, string firstName, string lastName, string emildID, int userRoleID, List<string> officesID)
        {
            var userManager = new DataAccess.UserManager();
            var status = userManager.UpdateUser(id, firstName, lastName, emildID, userRoleID, officesID);
            return Json(status);
        }

        [HttpPost]
        public JsonResult UpdateProfileImage()
        {
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
                        var userManager = new DataAccess.UserManager();
                        status = userManager.UpdateUser(Convert.ToInt32(Request.Form["id"]), Request.Form["firstName"].ToString(), Request.Form["lastName"].ToString(), Request.Form["emildID"].ToString(), Convert.ToInt32(Request.Form["userRoleID"]), Request.Form["officesID"].Split(',').ToList(), fileName, path);
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
            var userManager = new DataAccess.UserManager();
            var status = userManager.DeleteUser(id);
            return Json(status);
        }
        public PartialViewResult ChangePassword()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult UpdatePassword(string oldPassword, string newPassword)
        {
            var userManager = new DataAccess.UserManager();
            var status = userManager.UpdatePassword(oldPassword, newPassword, UserDetail.UserId);
            return Json(status);
        }
        public PartialViewResult DateDuration()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult AddDateDuration(DateTime startDate, DateTime endDate)
        {
            var userManager = new DataAccess.UserManager();
            var status = userManager.AddDateDuration(startDate, endDate, UserDetail.UserId);
            return Json(true);
        }
    }
}