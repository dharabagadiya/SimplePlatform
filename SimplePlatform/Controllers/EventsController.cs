using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class EventsController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Events", "Events.js", ControllerName);
            return View();
        }
        public ActionResult Add()
        {
            var userDetailManager = new UserManager();
            var officeMananer = new OfficeMananer();
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
        public JsonResult Add(string name, DateTime startDate, DateTime endDate, string description, int officeID)
        {
            var eventManager = new EventManager();
            return Json(eventManager.Add(name, startDate, endDate, description, officeID));
        }
        public JsonResult GetEvents()
        {
            var eventManager = new EventManager();
            var events = eventManager.GetEvents().Select(modal => new { id = modal.EventId, name = modal.Name, startDate = modal.StartDate.ToString("dd-MM-yyyy"), endDate = modal.EndDate.ToString("dd-MM-yyyy"), description = modal.Description }).ToList();
            return Json(new { data = events });
        }
        public PartialViewResult Edit(int id)
        {
            var userDetailManager = new UserManager();
            var officeMananer = new OfficeMananer();
            var user = userDetailManager.GetUserDetail(UserDetail.UserId);
            var offices = IsAdmin ? officeMananer.GetOffices() : user.Offices.ToList();
            ViewData["Offices"] = offices;
            var eventManager = new EventManager();
            var eventDetail = eventManager.GetEventDetail(id);
            return PartialView(eventDetail);
        }
        [HttpPost]
        public JsonResult Update(string name, DateTime startDate, DateTime endDate, string description, int officeID,int eventID)
        {
            var eventManager = new EventManager();
            return Json(eventManager.Update(name, startDate, endDate, description, officeID, eventID));
        }
        public JsonResult Delete(int id)
        {
            var eventManager = new EventManager();
            var status = eventManager.Delete(id);
            return Json(status);
        }
    }
}