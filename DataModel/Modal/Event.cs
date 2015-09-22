using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Modal
{
    public class Event
    {
        public int EventId { get; set; }
        public String Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Description { get; set; }
        public String City { get; set; }
        public virtual Office Office { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalAttended { get; set; }
        public virtual Convention convention { get; set; }
        public virtual ICollection<Audience> Audiences { get; set; }
    }
}
