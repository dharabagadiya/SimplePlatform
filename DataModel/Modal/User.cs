
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
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public String UserName { get; set; }
        [Required]
        public String Email { get; set; }
        [Required]
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Office> Offices { get; set; }
    }
}