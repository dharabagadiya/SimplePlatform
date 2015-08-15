namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201508150423_DataModel_Event_add_isdelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "IsDeleted");
        }
    }
}
