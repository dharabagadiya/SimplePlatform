namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Data_Modal_Office_IsDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offices", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offices", "IsDeleted");
        }
    }
}
