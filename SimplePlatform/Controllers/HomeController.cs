﻿
#region Using Namespaces
using CustomAuthentication.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

namespace SimplePlatform.Controllers
{
    [CustomAuthorize(Roles = "Admin, Offices, Employee")]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}