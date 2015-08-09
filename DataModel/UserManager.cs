using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Modal;

namespace DataModel
{
    public class UserManager
    {
        readonly DataContext Context = new DataContext();

        public static void OnModelCreating(DbModelBuilder modelBuilder)
        { }

        public List<User> GetUsers()
        { return Context.Users.Where(modal => modal.IsDeleted == false).ToList(); }
        public User GetUser(int id)
        { return Context.Users.Where(modal => modal.IsDeleted == false && modal.UserId == id).FirstOrDefault(); }
        public List<User> GetUsers(int roleID)
        { return Context.Users.Where(modal => modal.IsDeleted == false && modal.Roles.Any(roleModel => roleModel.RoleId == roleID)).ToList(); }
    }
}
