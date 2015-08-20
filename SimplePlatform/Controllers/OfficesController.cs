using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;

namespace SimplePlatform.Controllers
{
    public class OfficesController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddStyle("/Offices", "Offices.css", ControllerName);
            BundleConfig.AddStyle("/Offices", "Detail.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Offices.js", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Detail.js", ControllerName);
            return View();
        }

        [HttpPost]
        public JsonResult GetOffices(int pageNo = 1, int pageSize = 3)
        {
            var officesManager = new OfficeMananer();
            var offices = officesManager.GetOffices();
            var totalRecord = offices.Count();
            var filteredOffices = offices.Select(modal => new
            {
                ID = modal.OfficeId,
                Name = modal.Name,
                Fundraising = new { ActTotal = 5, Total = 10 },
                Task = new { ActTotal = 5, Total = 100 },
                Events = new { ActTotal = 5, Total = 100 },
                BookingInProcess = new { ActTotal = 5, Total = 100 },
                BookingConfirm = new { ActTotal = 5, Total = 100 }
            }).OrderBy(modal => modal.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                totalRecord = totalRecord,
                currentPage = pageNo,
                pageSize = pageSize,
                offices = filteredOffices
            });
        }

        public PartialViewResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var roleID = customRoleProvider.GetRole("Offices");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var Users = customMembershipProvider.GetUsers(roleID.RoleId);
            return PartialView(Users);
        }

        public PartialViewResult Edit(int id)
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var roleID = customRoleProvider.GetRole("Offices");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var Users = customMembershipProvider.GetUsers(roleID.RoleId);
            ViewData["Users"] = Users;
            var officesManager = new OfficeMananer();
            return PartialView(officesManager.GetOffice(id));
        }

        [HttpPost]
        public JsonResult Add(string name, string contactNo, string city, int userID)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Add(name, contactNo, city, userID));
        }

        [HttpPost]
        public JsonResult Update(int id, string name, string contactNo, string city, int userID)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Update(id, name, contactNo, city, userID));
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Delete(id));
        }

        public ActionResult Detail(int id)
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetTasks(int officeID)
        {
            var officesManager = new OfficeMananer();
            var tasks = officesManager.GetTasks(officeID).Select(model => new
            {
                ID = model.TaskId,
                Name = model.Name,
                EndDate = model.EndDate.ToString("dd-MM-yyyy"),
                Description=model.Description
            }).ToList();
            return Json(tasks);
        }
    }
}