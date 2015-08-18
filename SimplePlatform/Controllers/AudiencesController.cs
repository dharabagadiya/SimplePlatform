using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class AudiencesController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Audiences", "audiences.js", ControllerName);
            var officeManager = new DataModel.OfficeMananer();
            ViewData["Offices"] = officeManager.GetOffices();
            var eventManager = new DataModel.EventManager();
            ViewData["Events"] = eventManager.GetActiveEvents();
            var userManager = new DataModel.UserManager();
            ViewData["FSMUsers"] = userManager.GetUsers(4);
            var conventionManager = new DataModel.ConventionManager();
            ViewData["Convention"] = conventionManager.GetActiveConventions();
            return View();
        }

        public JsonResult GetAudiences()
        {
            var audienceManager = new DataModel.AudienceManager();
            var users = audienceManager.GetAudiences().Select(model => new
            {
                ID = model.AudienceID,
                Name = model.Name,
                Contact = model.Contact,
                VisitDate = model.VisitDate.ToString("MMM dd,yyyy"),
                VisitType = model.VisitType.VisitTypeName,
                EventName = (model.Event == null ? "-" : model.Event.Name),
                ConventionName = (model.Convention == null ? (model.Event == null ? "-" : model.Event.convention.Name) : model.Convention.Name),
                Status = model.ConvensionBooking == null ? "-" : model.ConvensionBooking.IsBooked ? "Booked" : "In Process"
            }).ToList();
            return Json(new { data = users });
        }

        public PartialViewResult Add()
        {
            var visitTypeManager = new DataModel.VisitTypeManager();
            ViewData["VisitTypes"] = visitTypeManager.GetVisitTypes();
            var officeManager = new DataModel.OfficeMananer();
            ViewData["Offices"] = officeManager.GetOffices();
            var eventManager = new DataModel.EventManager();
            ViewData["Events"] = eventManager.GetActiveEvents();
            var userManager = new DataModel.UserManager();
            ViewData["FSMUsers"] = userManager.GetUsers(4);
            var conventionManager = new DataModel.ConventionManager();
            ViewData["Convention"] = conventionManager.GetActiveConventions();
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name, string visitDate, string contact, int visitType, int officeID, int eventID, int convensionID, int fsmID, int bookingStatus, float donationAmount)
        {
            var audienceManager = new DataModel.AudienceManager();
            var visitDateTime = Convert.ToDateTime(visitDate);
            var status = audienceManager.Add(name, contact, visitDateTime, visitType, officeID, eventID, fsmID, convensionID, bookingStatus == 1, donationAmount);
            return Json(false);
        }
    }
}