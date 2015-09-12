using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ConventionManager
    {
        private DataContext Context = new DataContext();
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modal.UserDetail>()
                .HasMany(u => u.Conventions)
                .WithMany(r => r.UsersDetail)
                .Map(model =>
                {
                    model.ToTable("UserConventions");
                    model.MapLeftKey("UserId");
                    model.MapRightKey("ConventionId");
                });
        }
        public bool Add(string name, DateTime startDate, DateTime endDate, string description, int userID, string city)
        {
            try
            {
                var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                //if (users == null || users.Count <= 0) { return false; }
                Context.Conventions.Add(new Modal.Convention
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = description,
                    UsersDetail = users,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    City = city
                });
                var status = Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Add(string name, DateTime startDate, DateTime endDate, string description, int userID, string city, string path, string fileName)
        {
            try
            {
                var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                //if (users == null || users.Count <= 0) { return false; }
                Context.Conventions.Add(new Modal.Convention
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = description,
                    UsersDetail = users,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    City = city,
                    FileResource = new Modal.FileResource { path = path, name = fileName }
                });
                var status = Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddAttachment(int conventionID, string path, string fileName)
        {
            try
            {
                var convention = Context.Conventions.Where(modal => modal.ConventionId == conventionID).FirstOrDefault();
                Context.ConventionAttachments.Add(new Modal.ConventionAttachment
                {
                    Convention = convention,
                    FileResource = new Modal.FileResource { path = path, name = fileName }
                });
                var status = Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Convention> GetConventions()
        { return Context.Conventions.Where(model => model.IsDeleted == false).ToList(); }
        public Convention GetConventionDetail(int id)
        { return Context.Conventions.Where(modal => modal.ConventionId == id).FirstOrDefault(); }
        public List<ConventionAttachment> GetAttachmentListOfConvention(int id)
        { return GetConventionDetail(id).ConventionAttachments.ToList(); }
        public bool Update(string name, DateTime startDate, DateTime endDate, string description, int userID, int conventionID, string city)
        {
            try
            {
                var conventionDetail = Context.Conventions.Where(model => model.ConventionId == conventionID).FirstOrDefault();
                conventionDetail.UsersDetail.Remove(conventionDetail.UsersDetail.FirstOrDefault());
                Context.SaveChanges();
                var userDetail = GetUserDetail(userID);
                //if (userDetail == null) { return false; }
                if (conventionDetail == null) return false;
                conventionDetail.Name = name;
                conventionDetail.StartDate = startDate;
                conventionDetail.EndDate = endDate;
                conventionDetail.Description = description;
                conventionDetail.City = city;
                conventionDetail.UpdateDate = DateTime.Now;
                conventionDetail.UsersDetail = new List<UserDetail> { userDetail };
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(string name, DateTime startDate, DateTime endDate, string description, int userID, int conventionID, string city, string path, string fileName)
        {
            try
            {
                var conventionDetail = Context.Conventions.Where(model => model.ConventionId == conventionID).FirstOrDefault();
                conventionDetail.UsersDetail.Remove(conventionDetail.UsersDetail.FirstOrDefault());
                Context.SaveChanges();
                var userDetail = GetUserDetail(userID);
                //if (userDetail == null) { return false; }
                if (conventionDetail == null) return false;
                conventionDetail.Name = name;
                conventionDetail.StartDate = startDate;
                conventionDetail.EndDate = endDate;
                conventionDetail.Description = description;
                conventionDetail.City = city;
                conventionDetail.UpdateDate = DateTime.Now;
                conventionDetail.UsersDetail = new List<UserDetail> { userDetail };
                if (!string.IsNullOrEmpty(path)) conventionDetail.FileResource = new Modal.FileResource { path = path, name = fileName };
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            var conventionDetail = Context.Conventions.Where(model => model.ConventionId == id).FirstOrDefault();
            if (conventionDetail == null) { return false; }
            conventionDetail.IsDeleted = true;
            Context.SaveChanges();
            return true;
        }
        public bool DeleteAttachment(int id, int conventionID)
        {
            var convention = Context.Conventions.Where(model => model.ConventionId == conventionID).FirstOrDefault();
            var conventionAttachment = convention.ConventionAttachments.Where(model => model.ConventionAttachmentId == id).FirstOrDefault();
            if (convention == null || conventionAttachment == null) { return false; }
            convention.ConventionAttachments.Remove(conventionAttachment);
            Context.SaveChanges();
            return true;
        }
        public UserDetail GetUserDetail(int id)
        { return Context.UsersDetail.Where(modal => modal.UserId == id).FirstOrDefault(); }
        public List<Convention> GetActiveConventions()
        { return Context.Conventions.Where(model => model.IsDeleted == false && DateTime.Compare(DateTime.Now, model.EndDate) > 0).ToList(); }
        public List<Audience> GetAudiences(int id)
        { return GetConventionDetail(id).Audiences.ToList(); }
        public List<Event> GetEvents(int id)
        { return GetConventionDetail(id).Events.ToList(); }
    }
}
