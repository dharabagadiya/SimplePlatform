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
            var officeManager = new DataAccess.OfficeMananer();
            var offices = officeManager.GetOffices(IsAdmin ? 0 : UserDetail.UserId);
            ViewData["Offices"] = offices;
            var eventManager = new DataAccess.EventManager();
            ViewData["Events"] = eventManager.GetActiveEvents(offices.Select(model => model.OfficeId).ToList());
            var userManager = new DataAccess.FSMDetailManager();
            ViewData["FSMUsers"] = userManager.FSMDetails();
            var conventionManager = new DataAccess.ConventionManager();
            ViewData["Convention"] = conventionManager.GetConventions();
            var serviceManager = new DataAccess.ServiceManager();
            ViewData["Services"] = serviceManager.GetServices();
            StartupScript = "audiences.DoPageSetting();";
            return View();
        }

        public JsonResult GetAudiences(string startDate, string endDate)
        {
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var officeManager = new DataAccess.OfficeMananer();
            var offices = officeManager.GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var audienceManager = new DataAccess.AudienceManager();
            var audiences = audienceManager.GetAudiences(offices.ToList(), startDateTime, endDateTime);
            if (audiences == null) { return Json(new { data = new { } }); }
            var users = audiences.Select(model => new
            {
                ID = model.AudienceID,
                Name = model.Name,
                Contact = model.Contact,
                EmailAddress = model.EmailAddress,
                VisitDate = model.VisitDate.ToString("MMM dd,yyyy"),
                VisitType = model.VisitType.VisitTypeName,
                EventName = (model.Event == null ? "-" : model.Event.Name),
                ConventionName = (model.Convention == null ? "-" : model.Convention.Name),
                ServiceName = (model.Service == null ? "-" : model.Service.ServiceName),
                Status = (model.VisitType.VisitTypeId == 1) ? "-" : (model.BookingStatus == 1 ? "In Progress" : (model.BookingStatus == 2 ? "Booked" : "Reach")),
                FSMName = model.FSMDetail == null ? " - " : model.FSMDetail.Name,
                Attended = model.IsAttended,
                GSBAmount = model.GSBAmount,
                DonationAmount = model.Amount
            }).ToList();

            return Json(new
            {
                data = users
            });
        }

        public PartialViewResult Edit(int id)
        {
            var visitTypeManager = new DataAccess.VisitTypeManager();
            ViewData["VisitTypes"] = visitTypeManager.GetVisitTypes();
            var officeManager = new DataAccess.OfficeMananer();
            var offices = officeManager.GetOffices(IsAdmin ? 0 : UserDetail.UserId);
            ViewData["Offices"] = offices;
            var eventManager = new DataAccess.EventManager();
            ViewData["Events"] = eventManager.GetActiveEvents(offices.Select(model => model.OfficeId).ToList());
            var userManager = new DataAccess.FSMDetailManager();
            ViewData["FSMUsers"] = userManager.FSMDetails();
            var conventionManager = new DataAccess.ConventionManager();
            ViewData["Convention"] = conventionManager.GetConventions();
            var serviceManager = new DataAccess.ServiceManager();
            ViewData["Services"] = serviceManager.GetServices();
            var audienceManager = new DataAccess.AudienceManager();
            var audience = audienceManager.GetAudience(id);
            return PartialView(audience);
        }

        public PartialViewResult Add()
        {
            var visitTypeManager = new DataAccess.VisitTypeManager();
            ViewData["VisitTypes"] = visitTypeManager.GetVisitTypes();
            var officeManager = new DataAccess.OfficeMananer();
            var offices = officeManager.GetOffices(IsAdmin ? 0 : UserDetail.UserId);
            ViewData["Offices"] = offices;
            var eventManager = new DataAccess.EventManager();
            ViewData["Events"] = eventManager.GetActiveEvents(offices.Select(model => model.OfficeId).ToList());
            var userManager = new DataAccess.FSMDetailManager();
            ViewData["FSMUsers"] = userManager.FSMDetails();
            var conventionManager = new DataAccess.ConventionManager();
            ViewData["Convention"] = conventionManager.GetConventions();
            var serviceManager = new DataAccess.ServiceManager();
            ViewData["Services"] = serviceManager.GetServices();
            return PartialView();
        }

        public PartialViewResult UpdateAudienceStatus()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name, string visitDate, string contact, string emailAddress, int visitType, int officeID, int eventID, int convensionID, int serviceID, int fsmID, int bookingStatus, float gsbAmount, float donationAmount)
        {
            var audienceManager = new DataAccess.AudienceManager();
            var visitDateTime = Convert.ToDateTime(visitDate);
            var status = audienceManager.Add(name, contact, emailAddress, visitDateTime, visitType, officeID, eventID, fsmID, convensionID, serviceID, bookingStatus, gsbAmount, donationAmount);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Update(int audienceID, string name, string visitDate, string contact, string emailAddress, int visitType, int officeID, int eventID, int convensionID, int serviceID, int fsmID, int bookingStatus, float gsbAmount, float donationAmount)
        {
            var audienceManager = new DataAccess.AudienceManager();
            var visitDateTime = Convert.ToDateTime(visitDate);
            var status = audienceManager.Update(audienceID, name, contact, emailAddress, visitDateTime, visitType, officeID, eventID, fsmID, convensionID, serviceID, bookingStatus, gsbAmount, donationAmount);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var audienceManager = new DataAccess.AudienceManager();
            var status = audienceManager.Delete(id);
            return Json(status);
        }

        [HttpPost]
        public JsonResult AttendStatus(int id, string arrivalDateTime)
        {
            var audienceManager = new DataAccess.AudienceManager();
            var status = audienceManager.AttendStatus(id, Convert.ToDateTime(arrivalDateTime));
            return Json(status);
        }


    }
}