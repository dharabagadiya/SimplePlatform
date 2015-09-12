using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class WarmUpController : Controller
    {
        public PartialViewResult WarmpUpEF()
        {
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var users = customMembershipProvider.GetUsers();
            var officeManager = new DataModel.OfficeMananer();
            var office = officeManager.GetOffices();
            return PartialView();
        }
    }
}