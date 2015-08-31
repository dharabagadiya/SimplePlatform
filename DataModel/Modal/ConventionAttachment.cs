
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel.Modal
{
    public class ConventionAttachment
    {
        [Key]
        public int ConventionAttachmentId { get; set; }
        public virtual Convention Convention { get; set; }
        public virtual FileResource FileResource { get; set; }
    }
}
