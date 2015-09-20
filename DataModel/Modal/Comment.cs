
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel.Modal
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public virtual UserDetail UserDetail { get; set; }
        public virtual Task Task { get; set; }
        public virtual ICollection<CommentAttachment> CommentAttachments { get; set; }
        [DefaultValue(false)]
        public bool IsFileAttached { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
