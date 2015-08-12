using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Modal
{
    public class Task
    {
        public int TaskId { get; set; }
        public String Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Description { get; set; }
        public virtual Office Office { get; set; }
        public virtual UserDetail UsersDetail { get; set; }
    }
}
