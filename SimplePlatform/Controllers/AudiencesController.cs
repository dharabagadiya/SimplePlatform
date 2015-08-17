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
            //var audienceManager = new DataModel.AudienceManager();
            //var users = audienceManager.GetAudiences().Select(model => new
            //{
            //    ID = model.AudienceID,
            //    Name = model.FirstName + " " + model.LastName,
            //    EmailID = model.EMailID,
            //    VisitType = model.VisitType.VisitTypeName,
            //    ConventionEventName = (model.Event == null ? (model.Convention == null ? "-" : model.Convention.Name) : model.Event.Name),
            //    Status = model.ConvensionBooking == null ?  "-" : model.ConvensionBooking.IsBooked ?  "Booked" :  "In Process"
            //}).ToList();
            return Json(null);
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
        public JsonResult Add(string firstName, string lastName, string emailID, int visitTypeID, int officeID, int eventID, int fsmID, int convensionID)
        {
            var audienceManager = new DataModel.AudienceManager();
            //var status = audienceManager.Add(firstName, lastName, emailID, visitTypeID, officeID, eventID, fsmID, convensionID);
            return Json(false);
        }
    }
}