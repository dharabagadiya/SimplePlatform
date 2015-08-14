namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Event_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Office_OfficeId = c.Int(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Offices", t => t.Office_OfficeId)
                .Index(t => t.Office_OfficeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Office_OfficeId", "dbo.Offices");
            DropIndex("dbo.Events", new[] { "Office_OfficeId" });
            DropTable("dbo.Events");
        }
    }
}
