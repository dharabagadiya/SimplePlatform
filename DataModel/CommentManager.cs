
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
    public class CommentManager
    {
        private DataContext Context = new DataContext();
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        { }

        public bool Add(int taskID, int userID, string message)
        {
            try
            {
                var userDetail = Context.UsersDetail.Where(model => model.UserId == userID).FirstOrDefault();
                if (userDetail == null) { return false; }
                var task = Context.Tasks.Where(model => model.TaskId == taskID).FirstOrDefault();
                if (task == null) { return false; }
                Context.Comments.Add(new Modal.Comment
                {
                    CommentText = message,
                    UserDetail = userDetail,
                    Task = task,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
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
