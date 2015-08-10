using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using CustomAuthentication;
using System.Collections.Generic;

namespace DataModel.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataModel.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataModel.DataContext context)
        {
            // Default Roles --- No Changes In Role List
            var userRoles = new List<Role>();
            userRoles.Add(new Role { RoleName = "Admin" });
            userRoles.Add(new Role { RoleName = "Offices" });
            userRoles.Add(new Role { RoleName = "Speakers" });
            userRoles.Add(new Role { RoleName = "Employee" });
            context.Roles.AddRange(userRoles);
            // Admin Default User Created --- Please Dnt Delete
            var user = new CustomAuthentication.User { UserName = "admin", Email = "admin@gmail.com", FirstName = "Admin", Password = "123456", IsActive = true, CreateDate = DateTime.UtcNow, Roles = new List<Role>() };
            user.Roles.Add(userRoles.Find(model => model.RoleName.Equals("Admin", StringComparison.InvariantCultureIgnoreCase)));
            if (context.Users.Any(model => model.UserName == user.UserName)) { return; }
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
