
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel
{
    public class TargetManager
    {
        private DataContext Context = new DataContext();

        public List<Modal.Target> GetTargets()
        {
            return Context.Targets.Where(modal => modal.IsDeleted == false).ToList();
        }
    }
}
