namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Office_Updated : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Offices");
            DropColumn("dbo.Offices", "ID"); 
            AddColumn("dbo.Offices", "OfficeId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Offices", "OfficeId");    
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offices", "ID", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Offices");
            DropColumn("dbo.Offices", "OfficeId");
            AddPrimaryKey("dbo.Offices", "ID");
        }
    }
}
