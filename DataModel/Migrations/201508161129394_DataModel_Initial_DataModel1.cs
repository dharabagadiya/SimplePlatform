namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Initial_DataModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audiences",
                c => new
                    {
                        AudienceID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EMailID = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Convention_ConventionId = c.Int(),
                        UserDetail_UserId = c.Int(),
                        VisitType_VisitTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.AudienceID)
                .ForeignKey("dbo.Conventions", t => t.Convention_ConventionId)
                .ForeignKey("dbo.UserDetails", t => t.UserDetail_UserId)
                .ForeignKey("dbo.VisitTypes", t => t.VisitType_VisitTypeId)
                .Index(t => t.Convention_ConventionId)
                .Index(t => t.UserDetail_UserId)
                .Index(t => t.VisitType_VisitTypeId);
            
            CreateTable(
                "dbo.VisitTypes",
                c => new
                    {
                        VisitTypeId = c.Int(nullable: false, identity: true),
                        VisitTypeName = c.String(),
                    })
                .PrimaryKey(t => t.VisitTypeId);
            
            CreateTable(
                "dbo.ConvensionBookings",
                c => new
                    {
                        ConvensionBookingID = c.Int(nullable: false, identity: true),
                        Amount = c.Single(nullable: false),
                        IsBooked = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Audience_AudienceID = c.Int(),
                    })
                .PrimaryKey(t => t.ConvensionBookingID)
                .ForeignKey("dbo.Audiences", t => t.Audience_AudienceID)
                .Index(t => t.Audience_AudienceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConvensionBookings", "Audience_AudienceID", "dbo.Audiences");
            DropForeignKey("dbo.Audiences", "VisitType_VisitTypeId", "dbo.VisitTypes");
            DropForeignKey("dbo.Audiences", "UserDetail_UserId", "dbo.UserDetails");
            DropForeignKey("dbo.Audiences", "Convention_ConventionId", "dbo.Conventions");
            DropIndex("dbo.ConvensionBookings", new[] { "Audience_AudienceID" });
            DropIndex("dbo.Audiences", new[] { "VisitType_VisitTypeId" });
            DropIndex("dbo.Audiences", new[] { "UserDetail_UserId" });
            DropIndex("dbo.Audiences", new[] { "Convention_ConventionId" });
            DropTable("dbo.ConvensionBookings");
            DropTable("dbo.VisitTypes");
            DropTable("dbo.Audiences");
        }
    }
}
