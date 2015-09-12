
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
    public class CommentAttachment
    {
        [Key]
        public int CommentAttachmentId { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual FileResource FileResource { get; set; }
    }
}
