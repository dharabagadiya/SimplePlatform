using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class ConventionsController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Conventions", "Convention.js", ControllerName);
            return View();
        }
        public ActionResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            //var roleID = customRoleProvider.GetRole("Speakers");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            //var Users = customMembershipProvider.GetUsers(roleID.RoleId);
            return PartialView();
        }
        [HttpPost]
        public JsonResult Add(string name, DateTime startDate, DateTime endDate, string description, int userId, string city)
        {
            var conventionManager = new ConventionManager();
            return Json(conventionManager.Add(name, startDate, endDate, description, userId, city));
        }
        public JsonResult GetConventions()
        {
            var conventionManager = new ConventionManager();
            var events = conventionManager.GetConventions().Select(modal => new { id = modal.ConventionId, name = modal.Name, startDate = modal.StartDate.ToString("dd-MM-yyyy HH:mm"), endDate = modal.EndDate.ToString("dd-MM-yyyy HH:mm"), description = modal.Description }).ToList();
            return Json(new { data = events });
        }
        public PartialViewResult Edit(int id)
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var roleID = customRoleProvider.GetRole("Speakers");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var conventionManager = new ConventionManager();
            var conventionDetail = conventionManager.GetConventionDetail(id);
            return PartialView(conventionDetail);
        }
        [HttpPost]
        public JsonResult Update(string name, DateTime startDate, DateTime endDate, string description, int userID, int conventionID, string city)
        {
            var conventionManager = new ConventionManager();
            return Json(conventionManager.Update(name, startDate, endDate, description, userID, conventionID, city));
        }
        public JsonResult Delete(int id)
        {
            var conventionManager = new ConventionManager();
            var status = conventionManager.Delete(id);
            return Json(status);
        }

    }
}