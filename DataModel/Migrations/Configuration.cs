using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DataModel.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataModel.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataModel.DataContext context)
        { }
    }
}
