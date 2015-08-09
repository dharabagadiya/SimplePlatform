
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
#endregion

namespace CustomAuthentication
{
    public class CustomRoleProvider
    {
        readonly DataContext Context = new DataContext();
        public void CreateRole(string roleName, string description)
        {
            Role Role = null;
            Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
            if (Role == null)
            {
                Role NewRole = new Role
                {
                    RoleName = roleName,
                    Description = description
                };
                Context.Roles.Add(NewRole);
                Context.SaveChanges();
            }
        }
        public void AddUserToRole(string userName, string roleName)
        {
            User user = Context.Users.Where(Usr => userName.Contains(Usr.UserName)).FirstOrDefault();
            Role role = Context.Roles.Where(Rl => roleName.Contains(Rl.RoleName)).FirstOrDefault();

            if (!user.Roles.Contains(role))
            {
                user.Roles.Add(role);
                Context.SaveChanges();
            }

        }
        public bool DeleteRole(string roleName)
        {
            Role Role = null;
            Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
            if (Role != null)
            {
                Role.Users.Clear();
                Context.Roles.Remove(Role);
                Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        public List<Role> GetAllRoles()
        {
            return Context.Roles.ToList();
        }
        public Role GetRole(string roleName)
        {
            Role Role = null;
            Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
            return Role;
        }
        public bool IsUserInRole(string username, string roleName)
        {
            User User = Context.Users.FirstOrDefault(Usr => Usr.UserName == username);
            Role Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);

            if (Role == null && User != null)
            {
                return User.Roles.Contains(Role);
            }
            return false;
        }
    }
}