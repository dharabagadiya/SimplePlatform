using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Modal
{
    public class Convention
    {
        public int ConventionId { get; set; }
        public String Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Description { get; set; }
        public String City { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<UserDetail> UsersDetail { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual ICollection<Audience> Audiences { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
