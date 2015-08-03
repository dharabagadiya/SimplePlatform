
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using CustomAuthentication.Security;
#endregion

namespace SimplePlatform.Controllers
{
    [CustomAuthentic]
    abstract public class BaseController : Controller
    {
        #region Protected Ovveride Methods
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
        }
        #endregion
    }
}