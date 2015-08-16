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
            var visitTypes = visitTypeManager.GetVisitTypes();
            ViewData["VisitTypes"] = visitTypes;
            var eventManager = new DataModel.EventManager();
            ViewData["Events"] = eventManager.GetActiveEvents();
            return PartialView();
        }
    }
}