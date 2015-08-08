using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;

namespace SimplePlatform.Controllers
{
    public class OfficesController : BaseController
    {
        // GET: Office
        public ActionResult Index()
        {
            BundleConfig.AddStyle("/Offices", "Offices.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Offices.js", ControllerName);
            return View();
        }

        public PartialViewResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name, string contactNo, string city)
        {
            try
            {
                DataContext Context = new DataContext();
                Context.Offices.Add(new Office
                {
                    Name = name,
                    ContactNo = contactNo,
                    City = city
                });
                var status = Context.SaveChanges();
                return Json(true);
            }
            catch {
                return Json(false);
            }
        }
    }
}