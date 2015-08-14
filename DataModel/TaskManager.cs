using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class TaskManager
    {
        private DataContext Context = new DataContext();
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        { }

        public bool Add(string name, string startDates, string endDates, string description, int officeID, int userID)
        {
            try
            {
                Modal.Office office = null;
                Modal.UserDetail userDetail = null;
                if (userID != 0) { userDetail = Context.UsersDetail.Where(modal => modal.UserId == userID).FirstOrDefault(); }
                office = Context.Offices.Where(modal => modal.OfficeId == officeID).FirstOrDefault();
                Context.Tasks.Add(new Modal.Task
                {
                    Name = name,
                    StartDate = Convert.ToDateTime(startDates),
                    EndDate = Convert.ToDateTime(endDates),
                    Description = description,
                    Office = office,
                    UsersDetail = userDetail
                });
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Modal.Task> GetTasks()
        { return Context.Tasks.ToList(); }
    }
}