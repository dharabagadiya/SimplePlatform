namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Convention_Event : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "convention_ConventionId", c => c.Int());
            CreateIndex("dbo.Events", "convention_ConventionId");
            AddForeignKey("dbo.Events", "convention_ConventionId", "dbo.Conventions", "ConventionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "convention_ConventionId", "dbo.Conventions");
            DropIndex("dbo.Events", new[] { "convention_ConventionId" });
            DropColumn("dbo.Events", "convention_ConventionId");
        }
    }
}
