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

        [HttpPost]
        public JsonResult GetOffices(int pageNo = 1, int pageSize = 3)
        {
            var totalRecord = Context.Offices.Count();
            var offices = Context.Offices.Select(modal => new
            {
                ID = modal.ID,
                Name = modal.Name,
                Fundraising = new { ActTotal = 5, Total = 10 },
                Task = new { ActTotal = 5, Total = 100 },
                Events = new { ActTotal = 5, Total = 100 },
                BookingInProcess = new { ActTotal = 5, Total = 100 },
                BookingConfirm = new { ActTotal = 5, Total = 100 }
            }).OrderBy(modal => modal.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                totalRecord = totalRecord,
                currentPage = pageNo,
                pageSize = pageSize,
                offices = offices
            });
        }

        public PartialViewResult Add()
        {
            return PartialView();
        }

        public PartialViewResult Edit(int id)
        {
            var Office = Context.Offices.Where(ofc => ofc.ID == id).FirstOrDefault();
            return PartialView(Office);
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
        public JsonResult Update(int id, string name, string contactNo, string city)
        {
            try
            {
                var office = Context.Offices.Where(model => model.ID == id).FirstOrDefault();
                if (office == null) { return Json(false); }
                office.Name = name;
                office.ContactNo = contactNo;
                office.City = city;
                Context.SaveChanges();
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
                Office.IsDeleted = true;
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