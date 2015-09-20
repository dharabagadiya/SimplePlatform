using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DataAccess
{
    public class DBManager
    {
        public Database database { get; set; }

        static DBManager()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
        }

        public DBManager()
        {
            
            database = DatabaseFactory.CreateDatabase();
        }
    }
}
