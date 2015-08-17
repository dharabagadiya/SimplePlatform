namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Audience_Model : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ConvensionBookings", new[] { "Audience_AudienceID" });
            RenameColumn(table: "dbo.ConvensionBookings", name: "Audience_AudienceID", newName: "AudienceID");
            DropPrimaryKey("dbo.ConvensionBookings");
            AlterColumn("dbo.ConvensionBookings", "ConvensionBookingID", c => c.Int(nullable: false));
            AlterColumn("dbo.ConvensionBookings", "AudienceID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ConvensionBookings", "AudienceID");
            CreateIndex("dbo.ConvensionBookings", "AudienceID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ConvensionBookings", new[] { "AudienceID" });
            DropPrimaryKey("dbo.ConvensionBookings");
            AlterColumn("dbo.ConvensionBookings", "AudienceID", c => c.Int());
            AlterColumn("dbo.ConvensionBookings", "ConvensionBookingID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ConvensionBookings", "ConvensionBookingID");
            RenameColumn(table: "dbo.ConvensionBookings", name: "AudienceID", newName: "Audience_AudienceID");
            CreateIndex("dbo.ConvensionBookings", "Audience_AudienceID");
        }
    }
}
