namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Audience_GSBAmount_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Audiences", "GSBAmount", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Audiences", "GSBAmount");
        }
    }
}
