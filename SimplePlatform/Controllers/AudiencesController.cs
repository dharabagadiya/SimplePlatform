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
                Status = model.IsBooked ? "Booked" : "In Process",
                FSMName = string.IsNullOrWhiteSpace(model.FSMName) ? " - " : model.FSMName,
                GSBAmount = model.GSBAmount,
                DonationAmount = model.Amount
            }).ToList();
            return Json(new { data = users });
        }

        public PartialViewResult Edit(int id)
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
            var audienceManager = new DataModel.AudienceManager();
            var audience = audienceManager.GetAudience(id);
            return PartialView(audience);
        }

        public PartialViewResult Add()
        {
            var visitTypeManager = new DataModel.VisitTypeManager();
            ViewData["VisitTypes"] = visitTypeManager.GetVisitTypes();
            var officeManager = new DataModel.OfficeMananer();
            ViewData["Offices"] = officeManager.GetOffices();
            var eventManager = new DataModel.EventManager();
            ViewData["Events"] = eventManager.GetActiveEvents();
            //var userManager = new DataModel.UserManager();
            //ViewData["FSMUsers"] = userManager.GetUsers(4);
            var conventionManager = new DataModel.ConventionManager();
            ViewData["Convention"] = conventionManager.GetActiveConventions();
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name, string visitDate, string contact, int visitType, int officeID, int eventID, int convensionID, string fsmName, int bookingStatus, float gsbAmount, float donationAmount)
        {
            var audienceManager = new DataModel.AudienceManager();
            var visitDateTime = Convert.ToDateTime(visitDate);
            var status = audienceManager.Add(name, contact, visitDateTime, visitType, officeID, eventID, fsmName, convensionID, bookingStatus == 1, gsbAmount, donationAmount);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Update(int audienceID, string name, string visitDate, string contact, int visitType, int officeID, int eventID, int convensionID, string fsmName, int bookingStatus, float gsbAmount, float donationAmount)
        {
            var audienceManager = new DataModel.AudienceManager();
            var visitDateTime = Convert.ToDateTime(visitDate);
            var status = audienceManager.Update(audienceID, name, contact, visitDateTime, visitType, officeID, eventID, fsmName, convensionID, bookingStatus == 1, gsbAmount, donationAmount);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var audienceManager = new DataModel.AudienceManager();
            var status = audienceManager.Delete(id);
            return Json(status);
        }
    }
}