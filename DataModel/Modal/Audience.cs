
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel.Modal
{
    public class Audience
    {
        public int AudienceID { get; set; }
        public string Name { get; set; }
        public DateTime VisitDate { get; set; }
        public string Contact { get; set; }
        public string EmailAddress { get; set; }
        public float GSBAmount { get; set; }
        public virtual Convention Convention { get; set; }
        public virtual VisitType VisitType { get; set; }
        public virtual FSMDetail FSMDetail { get; set; }
        public virtual string FSMName { get; set; }
        public float Amount { get; set; }
        public int BookingStatus { get; set; }
        public virtual Office Office { get; set; }
        public virtual Event Event { get; set; }
        public virtual Service Service { get; set; }
        [DefaultValue(false)]
        public bool IsAttended { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
