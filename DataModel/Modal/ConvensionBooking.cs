using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Modal
{
    public class ConvensionBooking
    {
        public int ConvensionBookingID { get; set; }
        public float Amount { get; set; }
        public virtual Audience Audience { get; set; }
        public bool IsBooked { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
