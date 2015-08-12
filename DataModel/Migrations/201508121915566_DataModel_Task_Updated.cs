namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Task_Updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tasks", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Tasks", "DueDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "DueDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Tasks", "EndDate");
            DropColumn("dbo.Tasks", "StartDate");
        }
    }
}
