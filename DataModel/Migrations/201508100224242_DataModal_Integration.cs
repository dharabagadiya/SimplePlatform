namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModal_Integration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offices",
                c => new
                    {
                        OfficeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ContactNo = c.String(),
                        City = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OfficeId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Office_OfficeId = c.Int(),
                        UsersDetail_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Offices", t => t.Office_OfficeId)
                .ForeignKey("dbo.UserDetails", t => t.UsersDetail_UserId)
                .Index(t => t.Office_OfficeId)
                .Index(t => t.UsersDetail_UserId);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserOffices",
                c => new
                    {
                        OfficeId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OfficeId, t.UserId })
                .ForeignKey("dbo.UserDetails", t => t.OfficeId, cascadeDelete: true)
                .ForeignKey("dbo.Offices", t => t.UserId, cascadeDelete: true)
                .Index(t => t.OfficeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "UsersDetail_UserId", "dbo.UserDetails");
            DropForeignKey("dbo.UserOffices", "UserId", "dbo.Offices");
            DropForeignKey("dbo.UserOffices", "OfficeId", "dbo.UserDetails");
            DropForeignKey("dbo.Tasks", "Office_OfficeId", "dbo.Offices");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserOffices", new[] { "UserId" });
            DropIndex("dbo.UserOffices", new[] { "OfficeId" });
            DropIndex("dbo.UserDetails", new[] { "UserId" });
            DropIndex("dbo.Tasks", new[] { "UsersDetail_UserId" });
            DropIndex("dbo.Tasks", new[] { "Office_OfficeId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserOffices");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.UserDetails");
            DropTable("dbo.Tasks");
            DropTable("dbo.Offices");
        }
    }
}
