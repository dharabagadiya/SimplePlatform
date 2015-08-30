
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
        private DataContext Context = new DataContext();

        public static void OnModelCreating(DbModelBuilder modelBuilder)
        { }

        public List<int> GetConventionIDs(int year, int week)
        {
            var startDateTime = Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(year, week);
            var endDateTime = startDateTime.AddDays(6);
            return Context.Conventions.Where(model => model.IsDeleted == false
            && model.EndDate.Year == year
            && model.EndDate >= startDateTime && model.EndDate <= endDateTime).Select(model => model.ConventionId).ToList();
        }

        public bool Add(string name, string contact, DateTime visitDate, int visitTypeID, int officeID, int eventID, string fsmName, int conventionID, bool isBooked, float GSBAmount, float amount)
        {
            try
            {
                Modal.Office office = null;
                Modal.Event eventDetail = null;
                Modal.Convention convention = null;
                var visitType = Context.VisitTypes.Where(model => model.VisitTypeId == visitTypeID).FirstOrDefault();
                office = Context.Offices.Where(model => model.OfficeId == officeID && model.IsDeleted == false).FirstOrDefault();
                eventDetail = Context.Events.Where(model => model.EventId == eventID && model.IsDeleted == false).FirstOrDefault();
                convention = Context.Conventions.Where(model => model.ConventionId == conventionID && model.IsDeleted == false).FirstOrDefault();
                Context.Audiences.Add(new Modal.Audience
                {
                    Name = name,
                    Contact = contact,
                    VisitDate = visitDate,
                    VisitType = visitType,
                    Office = office,
                    Event = eventDetail,
                    FSMName = fsmName,
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

        public bool Update(int audienceID, string name, string contact, DateTime visitDate, int visitTypeID, int officeID, int eventID, string fsmName, int conventionID, bool isBooked, float GSBAmount, float amount)
        {
            try
            {
                Modal.Office office = null;
                Modal.Event eventDetail = null;
                Modal.Convention convention = null;
                var visitType = Context.VisitTypes.Where(model => model.VisitTypeId == visitTypeID).FirstOrDefault();
                office = Context.Offices.Where(model => model.OfficeId == officeID && model.IsDeleted == false).FirstOrDefault();
                eventDetail = Context.Events.Where(model => model.EventId == eventID && model.IsDeleted == false).FirstOrDefault();
                convention = Context.Conventions.Where(model => model.ConventionId == conventionID && model.IsDeleted == false).FirstOrDefault();
                var audience = GetAudience(audienceID);
                if (audience == null) { return false; }
                audience.Name = name;
                audience.Contact = contact;
                audience.VisitDate = visitDate;
                audience.VisitType = visitType;
                audience.Office = office;
                audience.Event = eventDetail;
                audience.FSMName = fsmName;
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

        public bool AttendStatus(int id)
        {

            try
            {
                var audience = GetAudience(id);
                if (audience == null) { return false; }
                audience.IsAttended = !audience.IsAttended;
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

        public DataModel.Modal.ChartSeries GetFundingTargetsAchived(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();
            var audiencesSeriesData = audiences
                .Where(model => model.VisitDate.Year == year && model.IsDeleted == false)
                .OrderBy(model => model.VisitDate)
                .GroupBy(model => Utilities.DateTimeUtilities.GetIso8601WeekOfYear(model.VisitDate))
                .Select(model => new Modal.ChartSeries.DataPoint { weekNumber = model.Key, x = (Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(year, model.Key).AddDays(6) - startYear).TotalMilliseconds, y = model.Sum(tempModel => tempModel.Amount) }).ToList();
            var totalTargetAchieved = audiencesSeriesData.Sum(model => model.y);
            return new Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }

        public DataModel.Modal.ChartSeries GetFundingTargetsAchived(List<Modal.Office> offices, DateTime startDate, DateTime endDate)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();
            var audiencesSeriesData = audiences
                .Where(model => model.VisitDate >= startDate && model.VisitDate <= endDate && model.IsDeleted == false)
                .OrderBy(model => model.VisitDate)
                .GroupBy(model => Utilities.DateTimeUtilities.GetIso8601WeekOfYear(model.VisitDate))
                .Select(model => new Modal.ChartSeries.DataPoint { weekNumber = model.Key, x = (Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(model.Last().VisitDate.Year, model.Key).AddDays(6) - startYear).TotalMilliseconds, y = model.Sum(tempModel => tempModel.Amount) }).ToList();
            var totalTargetAchieved = audiencesSeriesData.Sum(model => model.y);
            return new Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }

        public DataModel.Modal.ChartSeries GetGSBTargetsAchived(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();
            var audiencesSeriesData = audiences
                .Where(model => model.VisitDate.Year == year && model.IsDeleted == false)
                .OrderBy(model => model.VisitDate)
                .GroupBy(model => Utilities.DateTimeUtilities.GetIso8601WeekOfYear(model.VisitDate))
                .Select(model => new Modal.ChartSeries.DataPoint { weekNumber = model.Key, x = (Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(year, model.Key).AddDays(6) - startYear).TotalMilliseconds, y = model.Sum(tempModel => tempModel.GSBAmount) }).ToList();
            return new Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }

        public DataModel.Modal.ChartSeries GetGSBTargetsAchived(List<Modal.Office> offices, DateTime startDate, DateTime endDate)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();
            var audiencesSeriesData = audiences
                .Where(model => model.VisitDate >= startDate && model.VisitDate <= endDate && model.IsDeleted == false)
                .OrderBy(model => model.VisitDate)
                .GroupBy(model => Utilities.DateTimeUtilities.GetIso8601WeekOfYear(model.VisitDate))
                .Select(model => new Modal.ChartSeries.DataPoint { weekNumber = model.Key, x = (Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(model.Last().VisitDate.Year, model.Key).AddDays(6) - startYear).TotalMilliseconds, y = model.Sum(tempModel => tempModel.GSBAmount) }).ToList();
            return new Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }

        public DataModel.Modal.ChartSeries GetBookingTargetsAchived(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();
            var audiencesSeriesData = audiences
                .Where(model => model.VisitDate.Year == year && model.IsBooked == true && model.Convention != null && model.IsDeleted == false)
                .OrderBy(model => model.VisitDate)
                .GroupBy(model => Utilities.DateTimeUtilities.GetIso8601WeekOfYear(model.VisitDate))
                .Select(model => new DataModel.Modal.ChartSeries.DataPoint { weekNumber = model.Key, x = (Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(year, model.Key).AddDays(6) - startYear).TotalMilliseconds, y = model.Count() }).ToList();
            return new DataModel.Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }

        public DataModel.Modal.ChartSeries GetBookingTargetsAchived(List<Modal.Office> offices, DateTime startDate, DateTime endDate)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();
            var audiencesSeriesData = audiences
                .Where(model => model.VisitDate >= startDate && model.VisitDate <= endDate && model.IsBooked == true && model.Convention != null && model.IsDeleted == false)
                .OrderBy(model => model.VisitDate)
                .GroupBy(model => Utilities.DateTimeUtilities.GetIso8601WeekOfYear(model.VisitDate))
                .Select(model => new DataModel.Modal.ChartSeries.DataPoint { weekNumber = model.Key, x = (Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(model.Last().VisitDate.Year, model.Key).AddDays(6) - startYear).TotalMilliseconds, y = model.Count() }).ToList();
            return new DataModel.Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }

        public DataModel.Modal.ChartSeries GetArrivalTargetsAchived(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();
            var audiencesSeriesData = audiences
                .Where(model => model.VisitDate.Year == year && model.IsAttended == true && model.IsBooked == true && model.Convention != null && model.IsDeleted == false)
                .OrderBy(model => model.VisitDate)
                .GroupBy(model => Utilities.DateTimeUtilities.GetIso8601WeekOfYear(model.Convention.StartDate))
                .Select(model => new DataModel.Modal.ChartSeries.DataPoint { weekNumber = model.Key, x = (Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(year, model.Key).AddDays(6) - startYear).TotalMilliseconds, y = model.Count() }).ToList();
            return new Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }

        public DataModel.Modal.ChartSeries GetArrivalTargetsAchived(List<Modal.Office> offices, DateTime startDate, DateTime endDate)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var audiences = GetAudiences(offices).ToList();
            var audiencesSeriesData = audiences
                .Where(model => model.VisitDate >= startDate && model.VisitDate <= endDate && model.IsAttended == true && model.IsBooked == true && model.Convention != null && model.IsDeleted == false)
                .OrderBy(model => model.VisitDate)
                .GroupBy(model => Utilities.DateTimeUtilities.GetIso8601WeekOfYear(model.Convention.StartDate))
                .Select(model => new DataModel.Modal.ChartSeries.DataPoint { weekNumber = model.Key, x = (Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(model.Last().VisitDate.Year, model.Key).AddDays(6) - startYear).TotalMilliseconds, y = model.Count() }).ToList();
            return new Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = audiencesSeriesData };
        }

        public List<Modal.Audience> GetArrivalAudiences(int officeID, int year, int week)
        {
            var conventions = GetConventionIDs(year, week);
            return Context.Audiences.Where(model => model.IsDeleted == false
            && model.IsBooked == true
            && model.IsAttended == false
            && model.Office.OfficeId == officeID
            && conventions.Contains(model.Convention.ConventionId)).ToList();
        }
    }
}