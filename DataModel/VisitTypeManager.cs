using System;

#region Using Namespaces
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel
{
    public class VisitTypeManager
    {
        private DataContext Context = new DataContext();
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var visitTypes = new List<Modal.VisitType>();
            visitTypes.Add(new Modal.VisitType { VisitTypeName = "Office" });
            visitTypes.Add(new Modal.VisitType { VisitTypeName = "Event" });
            visitTypes.Add(new Modal.VisitType { VisitTypeName = "Convension" });
        }

        public List<Modal.VisitType> GetVisitTypes() {
            return Context.VisitTypes.ToList();
        }
    }
}
