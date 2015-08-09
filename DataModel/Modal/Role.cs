
#region Using Namespaces
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace DataModel.Modal
{
    public class Role
    {
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}