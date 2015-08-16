
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Modal
{
    public class VisitType
    {
        public int VisitTypeId { get; set; }

        public string VisitTypeName { get; set; }

        public virtual ICollection<Audience> Audiences { get; set; }
    }
}
