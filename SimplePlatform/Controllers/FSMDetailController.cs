
#region Using Namespaces
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

namespace SimplePlatform.Controllers
{
    public class FSMDetailController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/FSMDetail", "fsmdetail.js", ControllerName);
            StartupScript = "fsmdetail.DoPageSetting();";
            return View();
        }

        public JsonResult GetFSMUsers()
        {
            var fsmDetailManager = new FSMDetailManager();
            var fsmUsers = fsmDetailManager.FSMDetails();
            return Json(new { data = fsmUsers });
        }

        public PartialViewResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name, string emailID, string phoneNumber)
        {
            var fsmDetailManager = new FSMDetailManager();
            var status = fsmDetailManager.Add(name, emailID, phoneNumber);
            return Json(status);
        }

        public PartialViewResult Edit(int id)
        {
            var fsmDetailManager = new DataAccess.FSMDetailManager();
            var fsmDetail = fsmDetailManager.FSMDetail(id);
            return PartialView(fsmDetail);
        }

        [HttpPost]
        public JsonResult Update(int id, string name, string emailID, string phoneNumber)
        {
            var fsmDetailManager = new FSMDetailManager();
            var status = fsmDetailManager.Update(id, name, emailID, phoneNumber);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var fsmDetailManager = new FSMDetailManager();
            var status = fsmDetailManager.Delete(id);
            return Json(status);
        }
    }
}