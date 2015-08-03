
#region Using Namespaces
using CustomAuthentication;
using Security.Models;
using System.Web.Mvc;
using System.Web.Security;
#endregion

namespace SimplePlatform.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var customMembershipProvider = new CustomMembershipProvider();
                if (customMembershipProvider.Authenticate(model.Username, model.Password)) { return RedirectToAction("Index", "Home"); }
                ModelState.AddModelError("", "Incorrect username and/or password");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("", "Login");
        }
    }
}