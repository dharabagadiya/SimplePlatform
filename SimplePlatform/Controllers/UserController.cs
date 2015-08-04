using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            return PartialView(customRoleProvider.GetAllRoles());
        }
    }
}