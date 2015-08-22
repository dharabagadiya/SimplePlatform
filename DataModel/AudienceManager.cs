
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel
{
    public class AudienceManager
    {
        private static Calendar cal = CultureInfo.InvariantCulture.Calendar;

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = cal.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private DataContext Context = new DataContext();

        public static void OnModelCreating(DbModelBuilder modelBuilder)
        { }

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
                    GSBAmount = GSBAmount,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Amount = amount,
                    IsBooked = isBooked
                });
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
                audience.Amount = amount;
                audience.IsBooked = isBooked;
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

        public List<Modal.Audience> GetAudiences(List<Modal.Office> offices)
        {
            var officesID = offices.Select(model => model.OfficeId).ToList();
            return Context.Audiences.Where(modal => modal.IsDeleted == false && officesID.Contains(modal.Office.OfficeId)).ToList();
        }

        public object GetFundingTargets(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();

            var audiencesSeriesData = audiences.Where(model => model.IsBooked)
                .GroupBy(model => GetIso8601WeekOfYear(model.UpdateDate))
                .Select(model => new object[] {
                                           (model.LastOrDefault().UpdateDate - startYear).TotalMilliseconds,
                                           model.Sum(tempModel => tempModel.Amount)
                                       }).ToList();
            return new { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }
    }
}