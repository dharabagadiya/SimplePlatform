using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Modal;
using CustomAuthentication;

namespace DataModel
{
    public class UserManager
    {
        readonly DataContext Context = new DataContext();

        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetail>().HasKey(e => e.UserId);
            modelBuilder.Entity<UserDetail>().HasRequired(e => e.User).WithRequiredDependent(model => (UserDetail)model.UserDetail);
        }

        public bool CreateUser(string firstName, string lastName, string emildID, int userRoleID)
        {
            try
            {
                if (Context.Users.Any(model => model.UserName.Equals(emildID, StringComparison.InvariantCultureIgnoreCase))) { return false; }
                var roles = Context.Roles.Where(model => model.RoleId == userRoleID).ToList();
                var user = new CustomAuthentication.User { UserName = emildID, Password = "12345", Email = emildID, FirstName = firstName, LastName = lastName, Roles = roles, CreateDate = DateTime.UtcNow };
                Context.UsersDetail.Add(new UserDetail { User = user });
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateUser(int id, string firstName, string lastName, string emildID, int userRoleID)
        {
            try
            {
                var userDetail = GetUserDetail(id);
                if (userDetail == null) { return false; }
                userDetail.User.Roles.Remove(userDetail.User.Roles.FirstOrDefault());
                Context.SaveChanges();
                var roles = Context.Roles.Where(model => model.RoleId == userRoleID).ToList();
                userDetail.User.UserName = emildID;
                userDetail.User.Email = emildID;
                userDetail.User.FirstName = firstName;
                userDetail.User.LastName = lastName;
                userDetail.User.Roles = roles;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool DeleteUser(int id)
        {
            var userDetail = GetUserDetail(id);
            if (userDetail != null)
            {
                userDetail.User.IsDeleted = true;
                Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<User> GetUsers(int roleID)
        { return Context.Users.Where(modal => modal.Roles.Any(roleModel => roleModel.RoleId == roleID)).ToList(); }
        public List<UserDetail> GetUsersDetail()
        { return Context.UsersDetail.ToList(); }
        public UserDetail GetUserDetail(int id)
        { return Context.UsersDetail.Where(modal => modal.UserId == id).FirstOrDefault(); }
    }
}
