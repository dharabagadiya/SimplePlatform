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
        DataContext Context = new DataContext();
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
                Context.Offices.Add(new Office
                {
                    Name = name,
                    ContactNo = contactNo,
                    City = city
                });
                var status = Context.SaveChanges();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var Office = Context.Offices.Where(ofc => ofc.ID == id).FirstOrDefault();
            if (Office != null)
            {
                Context.Offices.Remove(Office);
                Context.SaveChanges();
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}