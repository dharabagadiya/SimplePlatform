using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class FileResourceManager
    {
        private DataContext Context = new DataContext();
        public bool Add(string path)
        {
            try
            {
                Context.FileResources.Add(new Modal.FileResource
                {
                    path = path
                });
                var status = Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
