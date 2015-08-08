
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
        #region Public Members
        public string ControllerName { get; set; }
        public string Script { get { return ViewData["View_Script"].ToString(); } set { ViewData["View_Script"] += value; } }
        public string StartupScript { get { return ViewData["View_StartupScript"].ToString(); } set { ViewData["View_StartupScript"] += value; } }
        #endregion


        #region Protected Ovveride Methods
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ControllerName = filterContext.RouteData.Values["controller"].ToString();
            ViewData["Controller_Name"] = ControllerName;
            Script = string.Empty;
            StartupScript = string.Empty;
            base.OnActionExecuting(filterContext);
        }
        #endregion
    }
}