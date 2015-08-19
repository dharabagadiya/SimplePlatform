﻿
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
        }

        public bool Add(string name, string contact, DateTime visitDate, int visitTypeID, int officeID, int eventID, int fsmID, int conventionID, bool isBooked, float GSBAmount, float amount)
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
                var audience = new Modal.Audience
                {
                    Name = name,
                    Contact = contact,
                    VisitDate = visitDate,
                    VisitType = visitType,
                    Office = office,
                    Event = eventDetail,
                    UserDetail = userDetail,
                    Convention = convention,
                    GSBAmount = GSBAmount,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                Context.ConvensionBookings.Add(new Modal.ConvensionBooking { Audience = audience, Amount = amount, IsBooked = isBooked, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(int audienceID, string name, string contact, DateTime visitDate, int visitTypeID, int officeID, int eventID, int fsmID, int conventionID, bool isBooked, float GSBAmount, float amount)
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
                var audience = GetAudience(audienceID);
                if (audience == null) { return false; }
                audience.Name = name;
                audience.Contact = contact;
                audience.VisitDate = visitDate;
                audience.VisitType = visitType;
                audience.Office = office;
                audience.Event = eventDetail;
                audience.UserDetail = userDetail;
                audience.Convention = convention;
                audience.GSBAmount = GSBAmount;
                audience.UpdateDate = DateTime.Now;
                audience.ConvensionBooking.Amount = amount;
                audience.ConvensionBooking.IsBooked = isBooked;
                audience.ConvensionBooking.UpdateDate = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {

            try
            {
                var audience = GetAudience(id);
                if (audience == null) { return false; }
                audience.IsDeleted = true;
                audience.UpdateDate = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Modal.Audience> GetAudiences() { return Context.Audiences.Where(model => model.IsDeleted == false).ToList(); }
        public Modal.Audience GetAudience(int id) { return Context.Audiences.Where(model => model.AudienceID == id && model.IsDeleted == false).FirstOrDefault(); }
    }
}