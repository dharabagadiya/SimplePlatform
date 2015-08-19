
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
        public float GSBAmount { get; set; }
        public virtual Convention Convention { get; set; }
        public virtual VisitType VisitType { get; set; }
        public virtual UserDetail UserDetail { get; set; }
        public virtual ConvensionBooking ConvensionBooking { get; set; }
        public virtual Office Office { get; set; }
        public virtual Event Event { get; set; }
        [DefaultValue(false)]
        public bool IsAuttended { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
