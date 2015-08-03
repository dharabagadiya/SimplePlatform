
#region Using Namespaces
using CustomAuthentication.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
#endregion

namespace CustomAuthentication
{
    public class CustomMembershipProvider
    {
        readonly DataContext Context = new DataContext();

        #region Private Methods
        private void InitializeUserSession(User user)
        {
            var roles = user.Roles.Select(m => m.RoleName).ToArray();
            var serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.UserId = user.UserId;
            serializeModel.FirstName = user.FirstName;
            serializeModel.LastName = user.LastName;
            serializeModel.roles = roles;
            var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(15), false, JsonConvert.SerializeObject(serializeModel));
            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }
        #endregion

        public bool CreateUser(string username, string password, string email)
        {
            try
            {
                if (Context.Users.Any(model => model.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase))) { return false; }
                Context.Users.Add(new User
                {
                    UserName = username,
                    Password = password,
                    Email = email,
                    CreateDate = DateTime.UtcNow,
                });
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool Authenticate(string username, string password)
        {
            var user = Context.Users.Where(model => (model.UserName.Equals(model.UserName) || model.Email.Equals(model.UserName)) && model.Password.Equals(password)).FirstOrDefault();
            if (user == null) { return false; }
            else { InitializeUserSession(user); return true; };
        }
        public bool DeleteUser(string username)
        {
            var User = Context.Users.FirstOrDefault(Usr => Usr.UserName == username);
            if (User != null)
            {
                Context.Users.Remove(User);
                Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}