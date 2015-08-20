namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Targets_Model_Updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Targets", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Targets", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Targets", "UpdateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Targets", "UpdateDate");
            DropColumn("dbo.Targets", "CreateDate");
            DropColumn("dbo.Targets", "IsDeleted");
        }
    }
}
