namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Task_Changed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tasks", "UpdateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "UpdateDate");
            DropColumn("dbo.Tasks", "CreateDate");
        }
    }
}
