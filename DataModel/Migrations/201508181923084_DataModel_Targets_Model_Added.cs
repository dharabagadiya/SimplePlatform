namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Targets_Model_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Targets",
                c => new
                    {
                        TargetId = c.Int(nullable: false, identity: true),
                        Booking = c.Int(nullable: false),
                        FundRaising = c.Single(nullable: false),
                        GSB = c.Single(nullable: false),
                        Arrivals = c.Single(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Office_OfficeId = c.Int(),
                    })
                .PrimaryKey(t => t.TargetId)
                .ForeignKey("dbo.Offices", t => t.Office_OfficeId)
                .Index(t => t.Office_OfficeId);
            
            AddColumn("dbo.Audiences", "IsAuttended", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Targets", "Office_OfficeId", "dbo.Offices");
            DropIndex("dbo.Targets", new[] { "Office_OfficeId" });
            DropColumn("dbo.Audiences", "IsAuttended");
            DropTable("dbo.Targets");
        }
    }
}
