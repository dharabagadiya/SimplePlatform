using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Modal;

namespace DataModel
{
    public class UserDetailManager
    {
        readonly DataContext Context = new DataContext();

        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetail>().HasKey(e => e.UserId);
            // Configure StudentId as FK for StudentAddress
            modelBuilder.Entity<UserDetail>().HasRequired(ad => ad.User).WithOptional();
        }

        public List<UserDetail> GetUsersDetail()
        { return Context.UsersDetail.ToList(); }
        public UserDetail GetUserDetail(int id)
        { return Context.UsersDetail.Where(modal => modal.UserId == id).FirstOrDefault(); }
    }
}
