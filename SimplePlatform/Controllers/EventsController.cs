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
            StartupScript = "events.DoPageSetting()";
            return View();
        }
        public ActionResult Add()
        {
            var userDetailManager = new UserManager();
            var officeMananer = new OfficeMananer();
            var user = userDetailManager.GetUserDetail(UserDetail.UserId);
            var offices = IsAdmin ? officeMananer.GetOffices() : user.Offices.ToList();
            ViewData["Offices"] = offices;
            var conventionManager = new ConventionManager();
            var conventions = conventionManager.GetConventions();
            ViewData["Conventions"] = conventions;
            if (IsAdmin)
            {
                var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
                var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
                ViewData["Employee"] = customMembershipProvider.GetUsers(4);
            }
            return PartialView();
        }
        [HttpPost]
        public JsonResult Add(string name, DateTime startDate, DateTime endDate, string description, int officeID, int conventionID, string city)
        {
            var eventManager = new EventManager();
            return Json(eventManager.Add(name, startDate, endDate, description, officeID, conventionID, city));
        }
        public JsonResult GetEvents(string startDate, string endDate)
        {
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var isUpdateEnable = UserDetail.User.Roles.Any(role => new List<int> { 1, 2 }.Contains(role.RoleId));
            var officesManager = new OfficeMananer();
            var offices = IsAdmin ? officesManager.GetOffices() : UserDetail.Offices;
            var eventList = offices.Where(model => model.IsDeleted == false).SelectMany(model => model.Events).ToList();
            var events = eventList.Where(model => model.IsDeleted == false)
                .Where(model => model.StartDate >= startDateTime && model.StartDate <= endDateTime)
                .Select(modal => new
            {
                id = modal.EventId,
                name = modal.Name,
                startDate = modal.StartDate.ToString("dd-MM-yyyy HH:mm"),
                endDate = modal.EndDate.ToString("dd-MM-yyyy HH:mm"),
                description = modal.Description,
                city = modal.City,
                IsUpdateEnable = isUpdateEnable
            }).ToList();
            return Json(new { data = events });
        }
        public PartialViewResult Edit(int id)
        {
            var userDetailManager = new UserManager();
            var officeMananer = new OfficeMananer();
            var user = userDetailManager.GetUserDetail(UserDetail.UserId);
            var offices = IsAdmin ? officeMananer.GetOffices() : user.Offices.ToList();
            ViewData["Offices"] = offices;
            var conventionManager = new ConventionManager();
            var conventions = conventionManager.GetConventions();
            ViewData["Conventions"] = conventions;
            var eventManager = new EventManager();
            var eventDetail = eventManager.GetEventDetail(id);
            return PartialView(eventDetail);
        }
        [HttpPost]
        public JsonResult Update(string name, DateTime startDate, DateTime endDate, string description, int officeID, int eventID, int conventionID, string city)
        {
            var eventManager = new EventManager();
            return Json(eventManager.Update(name, startDate, endDate, description, officeID, eventID, conventionID, city));
        }
        public JsonResult Delete(int id)
        {
            var eventManager = new EventManager();
            var status = eventManager.Delete(id);
            return Json(status);
        }
        public ActionResult Detail(int id)
        {
            BundleConfig.AddScript("~/Scripts/Events", "Detail.js", ControllerName);
            var eventManager = new DataModel.EventManager();
            return View(eventManager.GetEventDetail(id));
        }
        public JsonResult GetAudiences(int id)
        {
            var audienceManager = new DataModel.EventManager();
            var audiences = audienceManager.GetAudiences(id).Select(model => new
            {
                ID = model.AudienceID,
                Name = model.Name,
                Contact = model.Contact,
                VisitDate = model.VisitDate.ToString("MMM dd,yyyy"),
                ConventionName = (model.Convention == null ? (model.Event == null ? "-" : model.Event.convention.Name) : model.Convention.Name),
                Status = model.IsBooked ? "Booked" : "In Progress",
                FSMName = string.IsNullOrWhiteSpace(model.FSMName) ? " - " : model.FSMName,
                Attended = model.IsAttended,
                GSBAmount = model.GSBAmount,
                DonationAmount = model.Amount
            }).ToList();
            return Json(new { data = audiences });
        }
    }
}