namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Initial_DataModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conventions",
                c => new
                    {
                        ConventionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ConventionId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentText = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Task_TaskId = c.Int(),
                        UserDetail_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Tasks", t => t.Task_TaskId)
                .ForeignKey("dbo.UserDetails", t => t.UserDetail_UserId)
                .Index(t => t.Task_TaskId)
                .Index(t => t.UserDetail_UserId);
            
            CreateTable(
                "dbo.UserConventions",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ConventionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ConventionId })
                .ForeignKey("dbo.UserDetails", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Conventions", t => t.ConventionId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ConventionId);
            
            AddColumn("dbo.Events", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserDetail_UserId", "dbo.UserDetails");
            DropForeignKey("dbo.Comments", "Task_TaskId", "dbo.Tasks");
            DropForeignKey("dbo.UserConventions", "ConventionId", "dbo.Conventions");
            DropForeignKey("dbo.UserConventions", "UserId", "dbo.UserDetails");
            DropIndex("dbo.UserConventions", new[] { "ConventionId" });
            DropIndex("dbo.UserConventions", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "UserDetail_UserId" });
            DropIndex("dbo.Comments", new[] { "Task_TaskId" });
            DropColumn("dbo.Events", "IsDeleted");
            DropTable("dbo.UserConventions");
            DropTable("dbo.Comments");
            DropTable("dbo.Conventions");
        }
    }
}
