
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
            return View();
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
    }
}