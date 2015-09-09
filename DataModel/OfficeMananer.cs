using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OfficeMananer
    {
        private DataContext Context = new DataContext();
        public bool Add(string name, string contactNo, string city, int userID)
        {
            try
            {
                var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                if (users == null || users.Count <= 0) { return false; }
                if (Context.Offices.Any(model => model.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))) { return false; }
                Context.Offices.Add(new Modal.Office
                {
                    Name = name,
                    ContactNo = contactNo,
                    City = city,
                    UsersDetail = users
                });
                var status = Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Add(string name, string contactNo, string city, int userID, string path, string fileName)
        {
            try
            {
                var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                if (users == null || users.Count <= 0) { return false; }
                if (Context.Offices.Any(model => model.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))) { return false; }
                Context.Offices.Add(new Modal.Office
                {
                    Name = name,
                    ContactNo = contactNo,
                    City = city,
                    UsersDetail = users,
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
        public bool Update(int id, string name, string contactNo, string city, int userID)
        {
            try
            {
                var officeDetail = Context.Offices.Where(model => model.OfficeId == id).FirstOrDefault();
                officeDetail.UsersDetail.Remove(officeDetail.UsersDetail.FirstOrDefault());
                Context.SaveChanges();
                var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                if (users == null || users.Count <= 0) { return false; }
                var office = Context.Offices.Where(model => model.OfficeId == id).FirstOrDefault();
                if (office == null) { return false; }
                office.Name = name;
                office.ContactNo = contactNo;
                office.City = city;
                office.UsersDetail = users;
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(int id, string name, string contactNo, string city, int userID, string path)
        {
            try
            {
                var officeDetail = Context.Offices.Where(model => model.OfficeId == id).FirstOrDefault();
                officeDetail.UsersDetail.Remove(officeDetail.UsersDetail.FirstOrDefault());
                Context.SaveChanges();
                var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                if (users == null || users.Count <= 0) { return false; }
                var office = Context.Offices.Where(model => model.OfficeId == id).FirstOrDefault();
                if (office == null) { return false; }
                office.Name = name;
                office.ContactNo = contactNo;
                office.City = city;
                office.UsersDetail = users;
                if (!string.IsNullOrEmpty(path)) office.FileResource = new Modal.FileResource { path = path };
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
            var Office = Context.Offices.Where(ofc => ofc.OfficeId == id).FirstOrDefault();
            if (Office == null) { return false; }
            Office.IsDeleted = true;
            Context.SaveChanges();
            return true;
        }
        public Modal.Office GetOffice(int id)
        {
            return Context.Offices.Where(modal => modal.OfficeId == id).FirstOrDefault();
        }
        public List<Modal.Office> GetOffices()
        {
            return Context.Offices.Where(modal => modal.IsDeleted == false).ToList();
        }
        public List<Modal.Task> GetTasks(int officeID)
        {
            return Context.Tasks.Where(model => model.Office.OfficeId == officeID && model.UsersDetail == null).OrderByDescending(model => model.EndDate).ToList();
        }
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modal.UserDetail>()
                .HasMany(u => u.Offices)
                .WithMany(r => r.UsersDetail)
                .Map(model =>
                {
                    model.ToTable("UserOffices");
                    model.MapLeftKey("UserId");
                    model.MapRightKey("OfficeId");
                });
        }
    }
}
