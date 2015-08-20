using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Modal
{
    public class Target
    {
        public int TargetId { get; set; }
        public int Booking { get; set; }
        public float FundRaising { get; set; }
        public float GSB { get; set; }
        public float Arrivals { get; set; }
        public DateTime DueDate { get; set; }
        public virtual Office Office { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
