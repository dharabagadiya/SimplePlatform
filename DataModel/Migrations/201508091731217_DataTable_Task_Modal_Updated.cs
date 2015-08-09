namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataTable_Task_Modal_Updated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserTasks", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.UserTasks", "Task_TaskId", "dbo.Tasks");
            DropIndex("dbo.UserTasks", new[] { "User_UserId" });
            DropIndex("dbo.UserTasks", new[] { "Task_TaskId" });
            AddColumn("dbo.Tasks", "User_UserId", c => c.Int());
            CreateIndex("dbo.Tasks", "User_UserId");
            AddForeignKey("dbo.Tasks", "User_UserId", "dbo.Users", "UserId");
            DropTable("dbo.UserTasks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Task_TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Task_TaskId });
            
            DropForeignKey("dbo.Tasks", "User_UserId", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "User_UserId" });
            DropColumn("dbo.Tasks", "User_UserId");
            CreateIndex("dbo.UserTasks", "Task_TaskId");
            CreateIndex("dbo.UserTasks", "User_UserId");
            AddForeignKey("dbo.UserTasks", "Task_TaskId", "dbo.Tasks", "TaskId", cascadeDelete: true);
            AddForeignKey("dbo.UserTasks", "User_UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
