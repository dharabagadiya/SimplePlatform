 
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
#endregion

namespace DataModel.Modal
{
    public class UserDetail : CustomAuthentication.IUserDetail
    {
        public int UserId { get; set; }
        public virtual CustomAuthentication.User User { get; set; }
        public virtual ICollection<Office> Offices { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Convention> Conventions { get; set; }
        public virtual ICollection<Audience> Audience { get; set; }
    }
}