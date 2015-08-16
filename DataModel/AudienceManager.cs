
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
        { }

        public bool Add(string firstName, string lastName, string emailID, int visitTypeID, int officeID, int eventID, int fsmID, int conventionID)
        {
            try
            {
                Modal.Office office = null;
                Modal.Event eventDetail = null;
                Modal.UserDetail userDetail = null;
                Modal.Convention convention = null;

                var visitType = Context.VisitTypes.Where(model => model.VisitTypeId == visitTypeID).FirstOrDefault();

                if (officeID != 0) { Context.Offices.Where(model => model.OfficeId == officeID && model.IsDeleted == false).FirstOrDefault(); }
                if (eventID != 0) { Context.Events.Where(model => model.EventId == eventID && model.IsDeleted == false).FirstOrDefault(); }
                if (eventID != 0) { Context.Events.Where(model => model.EventId == eventID && model.IsDeleted == false).FirstOrDefault(); }

                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
