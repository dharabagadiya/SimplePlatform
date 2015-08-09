namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataTable_User_Migration : DbMigration
    {
        public override void Up()
        {   
            CreateTable(
                "dbo.UserOffices",
                c => new
                    {
                        OfficeId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OfficeId, t.UserId })
                .ForeignKey("dbo.Users", t => t.OfficeId, cascadeDelete: true)
                .ForeignKey("dbo.Offices", t => t.UserId, cascadeDelete: true)
                .Index(t => t.OfficeId)
                .Index(t => t.UserId);            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserOffices", "UserId", "dbo.Offices");
            DropForeignKey("dbo.UserOffices", "OfficeId", "dbo.Users");
            DropIndex("dbo.UserOffices", new[] { "UserId" });
            DropIndex("dbo.UserOffices", new[] { "OfficeId" });
            DropTable("dbo.UserOffices");
        }
    }
}
