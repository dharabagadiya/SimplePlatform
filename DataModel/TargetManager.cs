
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel
{
    public class TargetManager
    {
        private DataContext Context = new DataContext();

        public List<Modal.Target> GetTargets()
        {
            return Context.Targets.Where(modal => modal.IsDeleted == false).ToList();
        }
        public List<Modal.Target> GetTargets(List<Modal.Office> offices)
        {
            var officesID = offices.Select(model => model.OfficeId).ToList();
            return Context.Targets.Where(modal => modal.IsDeleted == false && officesID.Contains(modal.Office.OfficeId)).ToList();
        }

        public bool Add(int officeID, DateTime dueDate, int bookingTargets, float fundRaisingAmount, float gsbAmount, int arrivalTargets)
        {
            try
            {
                Modal.Office office = null;
                office = Context.Offices.Where(model => model.OfficeId == officeID && model.IsDeleted == false).FirstOrDefault();
                if (office == null) { return false; }
                office.Targets.Add(new Modal.Target
                {
                    DueDate = dueDate,
                    Booking = bookingTargets,
                    FundRaising = fundRaisingAmount,
                    GSB = gsbAmount,
                    Arrivals = arrivalTargets,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(int targetID, int officeID, DateTime dueDate, int bookingTargets, float fundRaisingAmount, float gsbAmount, int arrivalTargets)
        {
            try
            {
                var office = Context.Offices.Where(model => model.OfficeId == officeID && model.IsDeleted == false).FirstOrDefault();
                var target = Context.Targets.Where(model => model.TargetId == targetID && model.IsDeleted == false).FirstOrDefault();
                if (target == null) { return false; }
                target.DueDate = dueDate;
                target.Booking = bookingTargets;
                target.FundRaising = fundRaisingAmount;
                target.GSB = gsbAmount;
                target.Arrivals = arrivalTargets;
                target.UpdateDate = DateTime.Now;
                target.Office = office;
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
                var target = Context.Targets.Where(model => model.TargetId == id).FirstOrDefault();
                target.UpdateDate = DateTime.Now;
                target.IsDeleted = true;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Modal.Target GetTarget(int id)
        {
            return Context.Targets.Where(model => model.TargetId == id && model.IsDeleted == false).FirstOrDefault();
        }

        public object GetFundingTargets(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var targets = GetTargets(offices).ToList();
            var targetSeriesData = targets.OrderBy(model => model.DueDate).Select(model => new object[] { (model.DueDate - startYear).TotalMilliseconds, model.FundRaising }).ToList();
            return new { type = "line", name = "Tagert Year - " + DateTime.Now.Year, data = targetSeriesData };
        }

        public object GetBookingTargets(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var targets = GetTargets(offices).ToList();
            var targetSeriesData = targets.OrderBy(model => model.DueDate).Select(model => new object[] { (model.DueDate - startYear).TotalMilliseconds, model.Booking }).ToList();
            return new { type = "line", name = "Tagert Year - " + DateTime.Now.Year, data = targetSeriesData };
        }

        public object GetGSBTargets(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var targets = GetTargets(offices).ToList();
            var targetSeriesData = targets.OrderBy(model => model.DueDate).Select(model => new object[] { (model.DueDate - startYear).TotalMilliseconds, model.GSB }).ToList();
            return new { type = "line", name = "Tagert Year - " + DateTime.Now.Year, data = targetSeriesData };
        }

        public object GetArrivalTargets(List<Modal.Office> offices, int year)
        {
            var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var targets = GetTargets(offices).ToList();
            var targetSeriesData = targets.OrderBy(model => model.DueDate).Select(model => new object[] { (model.DueDate - startYear).TotalMilliseconds, model.Arrivals }).ToList();
            return new { type = "line", name = "Tagert Year - " + DateTime.Now.Year, data = targetSeriesData };
        }
    }
}
