namespace CustomAuthentication.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CustomAuthentication.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CustomAuthentication.DataContext context)
        {
            // Default Roles --- No Changes In Role List
            var userRoles = new List<Role>();
            userRoles.Add(new Role { RoleName = "Admin" });
            userRoles.Add(new Role { RoleName = "Offices FSMS" });
            userRoles.Add(new Role { RoleName = "Speakers" });
            userRoles.AddRange(userRoles);
            // Admin Default User Created --- Please Dnt Delete
            var user = new User { UserName = "admin", Email = "admin@gmail.com", FirstName = "Admin", Password = "123456", IsActive = true, CreateDate = DateTime.UtcNow, Roles = new List<Role>() };
            user.Roles.Add(userRoles.Find(model => model.RoleName.Equals("Admin", StringComparison.InvariantCultureIgnoreCase)));
            if (context.Users.Any(model => model.UserName == user.UserName)) { return; }
            context.Users.Add(user);
        }
    }
}
