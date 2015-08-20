namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Audience_Model_Wrong_ColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Audiences", "IsAttended", c => c.Boolean(nullable: false));
            DropColumn("dbo.Audiences", "IsAuttended");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Audiences", "IsAuttended", c => c.Boolean(nullable: false));
            DropColumn("dbo.Audiences", "IsAttended");
        }
    }
}
