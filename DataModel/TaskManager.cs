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
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modal.Task>()
                .HasMany(u => u.Users)
                .WithMany(r => r.Tasks).Map(model =>
                {
                    model.ToTable("UserTasks");
                    model.MapRightKey("TaskId");
                    model.MapLeftKey("UserId");
                });
        }
    }
}
