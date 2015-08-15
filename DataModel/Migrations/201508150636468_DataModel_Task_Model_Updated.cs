namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Task_Model_Updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "IsDeleted");
        }
    }
}
