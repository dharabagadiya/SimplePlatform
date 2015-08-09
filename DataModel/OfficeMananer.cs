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
        public bool Add(string name, string contactNo, string city)
        {
            try
            {
                if (Context.Offices.Any(model => model.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))) { return false; }
                Context.Offices.Add(new Modal.Office
                {
                    Name = name,
                    ContactNo = contactNo,
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
        public bool Update(int id, string name, string contactNo, string city)
        {
            try
            {
                var office = Context.Offices.Where(model => model.OfficeId == id).FirstOrDefault();
                if (office == null) { return false; }
                office.Name = name;
                office.ContactNo = contactNo;
                office.City = city;
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
        public Modal.Office GetOffice(int id) {
            return Context.Offices.Where(modal => modal.OfficeId == id).FirstOrDefault();
        }
        public List<Modal.Office> GetOffices()
        {
            return Context.Offices.Where(modal => modal.IsDeleted == false).ToList();
        }
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modal.User>()
                .HasMany(u => u.Offices)
                .WithMany(r => r.Users)
                .Map(model =>
                {
                    model.ToTable("UserOffices");
                    model.MapLeftKey("OfficeId");
                    model.MapRightKey("UserId");
                });
        }
    }
}
