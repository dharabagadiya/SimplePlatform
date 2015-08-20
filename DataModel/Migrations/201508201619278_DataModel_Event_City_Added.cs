namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Event_City_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "City");
        }
    }
}
