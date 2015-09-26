
#region Using Namespaces
using CustomAuthentication.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
#endregion

namespace CustomAuthentication
{
    public class CustomMembershipProvider : DataAccess.DBManager
    {
        #region Private Methods
        private void InitializeUserSession(User user)
        {
            var roles = user.Roles.Select(m => m.RoleName).ToArray();
            var serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.UserId = user.UserId;
            serializeModel.FirstName = user.FirstName;
            serializeModel.LastName = user.LastName;
            serializeModel.roles = roles;
            var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(60), false, JsonConvert.SerializeObject(serializeModel));
            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Session["User"] = user;
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }
        #endregion

        public bool Authenticate(string username, string password)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AuthenticateUsers]"))
                {
                    database.AddInParameter(command, "@username", DbType.String, username);
                    database.AddInParameter(command, "@password", DbType.String, password);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<User> GetUsers()
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetUsers]"))
                {
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var users = (from dataRow in dataTable.AsEnumerable()
                             select new User
                             {
                                 UserId = dataRow.Field<int>("UserId"),
                                 UserName = dataRow.Field<string>("UserName"),
                                 FirstName = dataRow.Field<string>("FirstName"),
                                 LastName = dataRow.Field<string>("LastName"),
                                 Email = dataRow.Field<string>("Email"),
                                 Roles = new List<Role> {
                                            new Role {
                                                RoleId = dataRow.Field<int>("RoleId"),
                                                RoleName = dataRow.Field<string>("RoleName")
                                            }
                                   }
                             }).ToList();
                return users;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public User GetUser(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetUserByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var user = (from dataRow in dataTable.AsEnumerable()
                             select new User
                             {
                                 UserId = dataRow.Field<int>("UserId"),
                                 UserName = dataRow.Field<string>("UserName"),
                                 FirstName = dataRow.Field<string>("FirstName"),
                                 LastName = dataRow.Field<string>("LastName"),
                                 Email = dataRow.Field<string>("Email"),
                                 Roles = new List<Role> {
                                            new Role {
                                                RoleId = dataRow.Field<int>("RoleId"),
                                                RoleName = dataRow.Field<string>("RoleName")
                                            }
                                   }
                             }).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<User> GetUsers(int roleID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetUserByRoleID]"))
                {
                    database.AddInParameter(command, "@RoleID", DbType.Int32, roleID);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var users = (from dataRow in dataTable.AsEnumerable()
                             select new User
                             {
                                 UserId = dataRow.Field<int>("UserId"),
                                 UserName = dataRow.Field<string>("UserName"),
                                 FirstName = dataRow.Field<string>("FirstName"),
                                 LastName = dataRow.Field<string>("LastName"),
                                 Email = dataRow.Field<string>("Email"),
                                 Roles = new List<Role> {
                                            new Role {
                                                RoleId = dataRow.Field<int>("RoleId"),
                                                RoleName = dataRow.Field<string>("RoleName")
                                            }
                                   }
                             }).ToList();
                return users;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}