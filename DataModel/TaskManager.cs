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
                    UsersDetail = userDetail,
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

        public bool Update(int taskID, string name, string startDates, string endDates, string description, int officeID, int userID)
        {
            try
            {
                var task = GetTask(taskID);
                if (task == null) { return false; }
                Modal.Office office = null;
                Modal.UserDetail userDetail = null;
                if (userID != 0) { userDetail = Context.UsersDetail.Where(modal => modal.UserId == userID).FirstOrDefault(); }
                office = Context.Offices.Where(modal => modal.OfficeId == officeID).FirstOrDefault();
                task.Name = name;
                task.StartDate = Convert.ToDateTime(startDates);
                task.EndDate = Convert.ToDateTime(endDates);
                task.Description = description;
                task.Office = office;
                task.UsersDetail = userDetail;
                task.UpdateDate = DateTime.Now;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int taskID)
        {
            try
            {
                var task = GetTask(taskID);
                if (task == null) { return false; }
                task.IsDeleted = true;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Status(int taskID)
        {
            try
            {
                var task = GetTask(taskID);
                if (task == null) { return false; }
                task.IsCompleted = !task.IsCompleted;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Modal.Task> GetTasks()
        { return Context.Tasks.Where(model => model.IsDeleted == false).ToList(); }

        public List<Modal.Task> GetTasks(DateTime startDate, DateTime endDate)
        { return Context.Tasks.Where(model => model.IsDeleted == false && (model.StartDate >= startDate && model.StartDate <= endDate)).ToList(); }

        public Modal.Task GetTask(int id)
        { return Context.Tasks.Where(model => model.TaskId == id && model.IsDeleted == false).FirstOrDefault(); }

        public List<Modal.Task> GetTasks(int officeID, int year, int week)
        {
            var startDateTime = Utilities.DateTimeUtilities.FirstDateOfWeekISO8601(year, week);
            var endDateTime = startDateTime.AddDays(6);
            return Context.Tasks.Where(model => (model.Office.OfficeId == officeID && model.IsDeleted == false && model.EndDate.Year == year && model.EndDate >= startDateTime && model.EndDate <= endDateTime)).ToList();
        }

        public List<Modal.Task> GetTasks(int officeID, DateTime startDate, DateTime endDate)
        {
            return Context.Tasks.Where(model => (model.Office.OfficeId == officeID && model.IsDeleted == false && model.StartDate >= startDate && model.StartDate <= endDate)).ToList();
        }
    }
}