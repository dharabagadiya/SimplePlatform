using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class TasksController : Controller
    {
        public ActionResult Index()
        { return View(); }

        public PartialViewResult Add()
        {
            var officeManager = new DataModel.OfficeMananer();
            var offices = officeManager.GetOffices();
            ViewData["Offices"] = offices;
            return PartialView();
        }
    }
}