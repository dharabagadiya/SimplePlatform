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
        public ActionResult Index()
        {
            BundleConfig.AddStyle("/Offices", "Offices.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Offices", "Offices.js", ControllerName);
            return View();
        }

        [HttpPost]
        public JsonResult GetOffices(int pageNo = 1, int pageSize = 3)
        {
            var officesManager = new OfficeMananer();
            var offices = officesManager.GetOffices();
            var totalRecord = offices.Count();
            var filteredOffices = offices.Select(modal => new
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
                offices = filteredOffices
            });
        }

        public PartialViewResult Add()
        {
            return PartialView();
        }

        public PartialViewResult Edit(int id)
        {
            var officesManager = new OfficeMananer();
            return PartialView(officesManager.GetOffice(id));
        }

        [HttpPost]
        public JsonResult Add(string name, string contactNo, string city)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Add(name, contactNo, city));
        }

        [HttpPost]
        public JsonResult Update(int id, string name, string contactNo, string city)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Update(id, name, contactNo, city));
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var officesManager = new OfficeMananer();
            return Json(officesManager.Delete(id));
        }
    }
}