using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class AudiencesController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
            return Json(null);
        }
    }
}