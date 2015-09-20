using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class CommentsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(int id, string message)
        {
            var commentManager = new DataAccess.CommentManager();
            var status = commentManager.Add(id, UserDetail.UserId, message, null);
            return Json(status);
        }
    }
}