
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

namespace SimplePlatform.Controllers
{
    public class ServicesController : BaseController
    {
        public ActionResult Index()
        {

            BundleConfig.AddScript("~/Scripts/Services", "services.js", ControllerName);
            StartupScript = "services.DoPageSetting();";
            return View();
        }

        public JsonResult GetServices()
        {
            var serviceManager = new DataAccess.ServiceManager();
            var services = serviceManager.GetServices();
            return Json(new { data = services });
        }

        public PartialViewResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name)
        {
            var serviceManager = new DataAccess.ServiceManager();
            var status = serviceManager.Add(name);
            return Json(status);
        }

        public PartialViewResult Edit(int id)
        {
            var serviceManager = new DataAccess.ServiceManager();
            var service = serviceManager.GetService(id);
            return PartialView(service);
        }

        [HttpPost]
        public JsonResult Update(int id, string name)
        {
            var serviceManager = new DataAccess.ServiceManager();
            var status = serviceManager.Update(id, name);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var serviceManager = new DataAccess.ServiceManager();
            var status = serviceManager.Delete(id);
            return Json(status);
        }
    }
}