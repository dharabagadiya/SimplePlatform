namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Comment_Added : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserDetail_UserId", "dbo.UserDetails");
            DropForeignKey("dbo.Comments", "Task_TaskId", "dbo.Tasks");
            DropIndex("dbo.Comments", new[] { "UserDetail_UserId" });
            DropIndex("dbo.Comments", new[] { "Task_TaskId" });
            DropTable("dbo.Comments");
        }
    }
}
