namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Task_Office_Mapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Office_OfficeId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Offices", t => t.Office_OfficeId)
                .Index(t => t.Office_OfficeId);
            
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Task_TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Task_TaskId })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.Task_TaskId, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Task_TaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTasks", "Task_TaskId", "dbo.Tasks");
            DropForeignKey("dbo.UserTasks", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "Office_OfficeId", "dbo.Offices");
            DropIndex("dbo.UserTasks", new[] { "Task_TaskId" });
            DropIndex("dbo.UserTasks", new[] { "User_UserId" });
            DropIndex("dbo.Tasks", new[] { "Office_OfficeId" });
            DropTable("dbo.UserTasks");
            DropTable("dbo.Tasks");
        }
    }
}
