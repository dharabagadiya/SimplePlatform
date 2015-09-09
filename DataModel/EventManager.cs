using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class EventManager
    {
        private DataContext Context = new DataContext();
        public bool Add(string name, DateTime startDate, DateTime endDate, string description, int officeID, int conventionID, string city)
        {
            try
            {
                //var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                //if (users == null || users.Count <= 0) { return false; }
                //if (Context.Offices.Any(model => model.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))) { return false; }
                var office = Context.Offices.Where(model => model.OfficeId == officeID).FirstOrDefault();
                var convention = Context.Conventions.Where(model => model.ConventionId == conventionID).FirstOrDefault();
                Context.Events.Add(new Modal.Event
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = description,
                    Office = office,
                    convention = convention,
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
        public List<Event> GetEvents()
        { return Context.Events.Where(model => model.IsDeleted == false).ToList(); }
        public Event GetEventDetail(int id)
        { return Context.Events.Where(modal => modal.EventId == id).FirstOrDefault(); }
        public bool Update(string name, DateTime startDate, DateTime endDate, string description, int officeID, int eventID, int conventionID, string city)
        {
            try
            {
                var eventDetail = Context.Events.Where(model => model.EventId == eventID).FirstOrDefault();
                var office = Context.Offices.Where(model => model.OfficeId == officeID).FirstOrDefault();
                var convention = Context.Conventions.Where(model => model.ConventionId == conventionID).FirstOrDefault();
                if (eventDetail == null) return false;
                eventDetail.Name = name;
                eventDetail.StartDate = startDate;
                eventDetail.EndDate = endDate;
                eventDetail.Description = description;
                eventDetail.Office = office;
                eventDetail.convention = convention;
                eventDetail.City = city;
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
            var eventDetail = Context.Events.Where(model => model.EventId == id).FirstOrDefault();
            if (eventDetail == null) { return false; }
            eventDetail.IsDeleted = true;
            Context.SaveChanges();
            return true;
        }
        public List<Event> GetActiveEvents()
        { return Context.Events.Where(model => model.IsDeleted == false && DateTime.Compare(DateTime.Now, model.EndDate) > 0).ToList(); }
        public List<Event> GetActiveEvents(List<int> officeIDs)
        { return Context.Events.Where(model => model.IsDeleted == false && DateTime.Compare(DateTime.Now, model.EndDate) > 0 && officeIDs.Contains(model.Office.OfficeId)).ToList(); }
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
