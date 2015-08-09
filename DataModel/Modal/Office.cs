﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Modal
{
    public class Office
    {
        public int OfficeId { get; set; }
        public String Name { get; set; }
        public String ContactNo { get; set; }
        public String City { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
