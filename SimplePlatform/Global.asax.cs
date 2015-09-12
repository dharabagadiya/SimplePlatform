
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Optimization;
using System.Net;
#endregion

namespace SimplePlatform
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer<DataModel.DataContext>(new DataModel.DataContextInitilizer());
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
        }

        void Application_End(object sender, EventArgs e)
        {
            WebClient http = new WebClient();
            string Result = http.DownloadString("http://www.soundexpansion.org/WarmUp/WarmpUpEF");
            Result = http.DownloadString("http://soundexpansion.org/thread.php");
        }
    }
}
