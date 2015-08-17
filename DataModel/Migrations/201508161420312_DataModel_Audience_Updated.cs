namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Audience_Updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Audiences", "Event_EventId", c => c.Int());
            AddColumn("dbo.Audiences", "Office_OfficeId", c => c.Int());
            CreateIndex("dbo.Audiences", "Event_EventId");
            CreateIndex("dbo.Audiences", "Office_OfficeId");
            AddForeignKey("dbo.Audiences", "Event_EventId", "dbo.Events", "EventId");
            AddForeignKey("dbo.Audiences", "Office_OfficeId", "dbo.Offices", "OfficeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Audiences", "Office_OfficeId", "dbo.Offices");
            DropForeignKey("dbo.Audiences", "Event_EventId", "dbo.Events");
            DropIndex("dbo.Audiences", new[] { "Office_OfficeId" });
            DropIndex("dbo.Audiences", new[] { "Event_EventId" });
            DropColumn("dbo.Audiences", "Office_OfficeId");
            DropColumn("dbo.Audiences", "Event_EventId");
        }
    }
}
