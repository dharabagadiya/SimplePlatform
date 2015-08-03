
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
#endregion

namespace CustomAuthentication
{
    public class DataContextInitilizer : CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        { }
    }
}