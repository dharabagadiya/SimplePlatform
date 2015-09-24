using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ConventionManager
    {
        private DataContext Context = new DataContext();
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modal.UserDetail>()
                .HasMany(u => u.Conventions)
                .WithMany(r => r.UsersDetail)
                .Map(model =>
                {
                    model.ToTable("UserConventions");
                    model.MapLeftKey("UserId");
                    model.MapRightKey("ConventionId");
                });
        }
        //public List<Convention> GetConventions()
        //{ return Context.Conventions.Where(model => model.IsDeleted == false).ToList(); }
        public Convention GetConventionDetail(int id)
        { return Context.Conventions.Where(modal => modal.ConventionId == id).FirstOrDefault(); }
        public List<ConventionAttachment> GetAttachmentListOfConvention(int id)
        { return GetConventionDetail(id).ConventionAttachments.ToList(); }
        public List<Convention> GetActiveConventions()
        { return Context.Conventions.Where(model => model.IsDeleted == false && DateTime.Compare(DateTime.Now, model.EndDate) > 0).ToList(); }

    }
}
