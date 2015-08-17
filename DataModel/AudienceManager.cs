
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel
{
    public class AudienceManager
    {
        private DataContext Context = new DataContext();

        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modal.ConvensionBooking>().HasKey(e => e.AudienceID);

            // Configure StudentId as FK for StudentAddress
            modelBuilder.Entity<Modal.Audience>()
                        .HasOptional(s => s.ConvensionBooking) // Mark StudentAddress is optional for Student
                        .WithRequired(ad => ad.Audience); // Create inverse relationship
        }

        public bool Add(string name, string contact, DateTime visitDate, int visitTypeID, int officeID, int eventID, int fsmID, int conventionID, bool isBooked, float amount)
        {
            try
            {
                Modal.Office office = null;
                Modal.Event eventDetail = null;
                Modal.UserDetail userDetail = null;
                Modal.Convention convention = null;
                var visitType = Context.VisitTypes.Where(model => model.VisitTypeId == visitTypeID).FirstOrDefault();
                office = Context.Offices.Where(model => model.OfficeId == officeID && model.IsDeleted == false).FirstOrDefault();
                eventDetail = Context.Events.Where(model => model.EventId == eventID && model.IsDeleted == false).FirstOrDefault();
                userDetail = Context.UsersDetail.Where(model => model.UserId == fsmID && model.User.IsDeleted == false).FirstOrDefault();
                convention = Context.Conventions.Where(model => model.ConventionId == conventionID && model.IsDeleted == false).FirstOrDefault();
                Context.Audiences.Add(new Modal.Audience
                {
                    Name = name,
                    Contact = contact,
                    VisitDate = visitDate,
                    VisitType = visitType,
                    Office = office,
                    Event = eventDetail,
                    UserDetail = userDetail,
                    Convention = convention,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    ConvensionBooking = new Modal.ConvensionBooking { Amount = amount, IsBooked = isBooked, CreateDate = DateTime.Now, UpdateDate = DateTime.Now }
                });
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Modal.Audience> GetAudiences() { return Context.Audiences.ToList(); }
    }
}