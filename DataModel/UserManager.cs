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

        public bool CreateUser(string firstName, string lastName, string emildID, int userRoleID, int officeID)
        {
            try
            {
                if (Context.Users.Any(model => model.UserName.Equals(emildID, StringComparison.InvariantCultureIgnoreCase))) { return false; }
                var offices = Context.Offices.Where(model => model.OfficeId == officeID).ToList();
                var roles = Context.Roles.Where(model => model.RoleId == userRoleID).ToList();
                var user = new User { UserName = emildID, Password = "12345", Email = emildID, FirstName = firstName, LastName = lastName, Roles = roles, CreateDate = DateTime.UtcNow };
                Context.UsersDetail.Add(new UserDetail { User = user, Offices = offices });
                Context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool UpdateUser(int id, string firstName, string lastName, string emildID, int userRoleID, int officeID)
        {
            try
            {
                var userDetail = GetUserDetail(id);
                if (userDetail == null) { return false; }
                userDetail.User.Roles.Remove(userDetail.User.Roles.FirstOrDefault());
                userDetail.Offices.Remove(userDetail.Offices.FirstOrDefault());
                Context.SaveChanges();
                var roles = Context.Roles.Where(model => model.RoleId == userRoleID).ToList();
                var offices = Context.Offices.Where(model => model.OfficeId == officeID).ToList();
                userDetail.User.UserName = emildID;
                userDetail.User.Email = emildID;
                userDetail.User.FirstName = firstName;
                userDetail.User.LastName = lastName;
                userDetail.User.Roles = roles;
                userDetail.Offices = offices;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool UpdateUser(int id, string firstName, string lastName, string emildID, int userRoleID, int officeID, string fileName, string path)
        {
            try
            {
                var userDetail = GetUserDetail(id);
                if (userDetail == null) { return false; }
                userDetail.User.Roles.Remove(userDetail.User.Roles.FirstOrDefault());
                userDetail.Offices.Remove(userDetail.Offices.FirstOrDefault());
                Context.SaveChanges();
                var roles = Context.Roles.Where(model => model.RoleId == userRoleID).ToList();
                var offices = Context.Offices.Where(model => model.OfficeId == officeID).ToList();
                userDetail.User.UserName = emildID;
                userDetail.User.Email = emildID;
                userDetail.User.FirstName = firstName;
                userDetail.User.LastName = lastName;
                userDetail.User.Roles = roles;
                userDetail.FileResource = new Modal.FileResource { path = path, name = fileName };
                userDetail.Offices = offices;
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
        public List<UserDetail> GetUsers(int roleID)
        { return Context.UsersDetail.Where(model => model.User.Roles.Any(roleModel => roleModel.RoleId == roleID) && model.User.IsDeleted == false).ToList(); }
        public List<UserDetail> GetUsersDetails()
        { return Context.UsersDetail.Where(model => model.User.IsDeleted == false).ToList(); }
        public UserDetail GetUserDetail(int id)
        { return Context.UsersDetail.Where(modal => modal.UserId == id).FirstOrDefault(); }
        public bool UpdatePassword(string oldPassword, string newPassword,int userID)
        {
            var userDetail = GetUserDetail(userID);
            if (userDetail == null) return false;
            if(!userDetail.User.Password.Equals(oldPassword)) return false;
            userDetail.User.Password = newPassword;
            Context.SaveChanges();
            return true;
        }
        public bool AddDateDuration(DateTime startDate,DateTime endDate, int userID)
        {
            var userDetail = GetUserDetail(userID);
            if (userDetail == null) return false;
            userDetail.StartDate = startDate;
            userDetail.EndDate = endDate;
            Context.SaveChanges();
            return true;
        }
    }
}
