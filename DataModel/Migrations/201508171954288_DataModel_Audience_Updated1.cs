namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Audience_Updated1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Audiences", "Name", c => c.String());
            AddColumn("dbo.Audiences", "VisitDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Audiences", "Contact", c => c.String());
            DropColumn("dbo.Audiences", "FirstName");
            DropColumn("dbo.Audiences", "LastName");
            DropColumn("dbo.Audiences", "EMailID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Audiences", "EMailID", c => c.String());
            AddColumn("dbo.Audiences", "LastName", c => c.String());
            AddColumn("dbo.Audiences", "FirstName", c => c.String());
            DropColumn("dbo.Audiences", "Contact");
            DropColumn("dbo.Audiences", "VisitDate");
            DropColumn("dbo.Audiences", "Name");
        }
    }
}
