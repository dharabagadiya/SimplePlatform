using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class EventManager
    {
        private DataContext Context = new DataContext();
        public bool Add(string name, DateTime startDate, DateTime endDate, string description, int officeID)
        {
            try
            {
                //var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                //if (users == null || users.Count <= 0) { return false; }
                //if (Context.Offices.Any(model => model.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))) { return false; }
                Context.Events.Add(new Modal.Event
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = description,
                });
                var status = Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
