﻿
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
        #region Private Members
        private static readonly string ADMIN_ROLE = "Admin";
        private DataModel.Modal.UserDetail user { get; set; }
        private bool isAdmin { get; set; }
        #endregion

        #region Public Members
        public DataModel.Modal.UserDetail UserDetail { get { return user; } }
        public bool IsAdmin { get { return isAdmin; } }
        public string ControllerName { get; set; }
        public string Script { get { return ViewData["View_Script"].ToString(); } set { ViewData["View_Script"] += value; } }
        public string StartupScript { get { return ViewData["View_StartupScript"].ToString(); } set { ViewData["View_StartupScript"] += value; } }
        #endregion


        #region Protected Ovveride Methods
        protected override void OnAuthentication(AuthenticationContext filterContext)
        { }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region User Specific Settings
            var sessionUser = (CustomAuthentication.User)(Session["User"]);
            var userManager = new DataModel.UserManager();
            user = userManager.GetUserDetail(sessionUser.UserId);
            isAdmin = user.User.Roles.Any(model => model.RoleName.Equals(ADMIN_ROLE, StringComparison.InvariantCultureIgnoreCase));
            #endregion

            ControllerName = filterContext.RouteData.Values["controller"].ToString();
            ViewData["Controller_Name"] = ControllerName;
            Script = string.Empty;
            StartupScript = string.Empty;
            base.OnActionExecuting(filterContext);
        }
        #endregion
    }
}