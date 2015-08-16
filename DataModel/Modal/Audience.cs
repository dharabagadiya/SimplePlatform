
#region Using Namespaces
using System;
using System.Collections.Generic;
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
        public string EMailID { get; set; }
        public virtual Convention Convention { get; set; }
        public virtual VisitType VisitType { get; set; }
        public virtual UserDetail UserDetail { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
