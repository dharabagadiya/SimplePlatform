using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Office
    {
        [Key]
        public int ID { get; set; }
        public String Name { get; set; }
        public String ContactNo { get; set; }
        public String City { get; set; }
    }
}
