using CustomAuthentication;
using CustomAuthentication.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace SimplePlatform
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer<DataContext>(new DataContextInitilizer());
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
        }

    }
}
