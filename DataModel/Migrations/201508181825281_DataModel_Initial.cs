namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataModel_Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audiences",
                c => new
                    {
                        AudienceID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        VisitDate = c.DateTime(nullable: false),
                        Contact = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Convention_ConventionId = c.Int(),
                        Event_EventId = c.Int(),
                        Office_OfficeId = c.Int(),
                        UserDetail_UserId = c.Int(),
                        VisitType_VisitTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.AudienceID)
                .ForeignKey("dbo.Conventions", t => t.Convention_ConventionId)
                .ForeignKey("dbo.Events", t => t.Event_EventId)
                .ForeignKey("dbo.Offices", t => t.Office_OfficeId)
                .ForeignKey("dbo.UserDetails", t => t.UserDetail_UserId)
                .ForeignKey("dbo.VisitTypes", t => t.VisitType_VisitTypeId)
                .Index(t => t.Convention_ConventionId)
                .Index(t => t.Event_EventId)
                .Index(t => t.Office_OfficeId)
                .Index(t => t.UserDetail_UserId)
                .Index(t => t.VisitType_VisitTypeId);
            
            CreateTable(
                "dbo.ConvensionBookings",
                c => new
                    {
                        AudienceID = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                        IsBooked = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AudienceID)
                .ForeignKey("dbo.Audiences", t => t.AudienceID)
                .Index(t => t.AudienceID);
            
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
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        convention_ConventionId = c.Int(),
                        Office_OfficeId = c.Int(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Conventions", t => t.convention_ConventionId)
                .ForeignKey("dbo.Offices", t => t.Office_OfficeId)
                .Index(t => t.convention_ConventionId)
                .Index(t => t.Office_OfficeId);
            
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
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UsersDetail_UserId = c.Int(),
                        Office_OfficeId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.UserDetails", t => t.UsersDetail_UserId)
                .ForeignKey("dbo.Offices", t => t.Office_OfficeId)
                .Index(t => t.UsersDetail_UserId)
                .Index(t => t.Office_OfficeId);
            
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
                "dbo.VisitTypes",
                c => new
                    {
                        VisitTypeId = c.Int(nullable: false, identity: true),
                        VisitTypeName = c.String(),
                    })
                .PrimaryKey(t => t.VisitTypeId);
            
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
            
            CreateTable(
                "dbo.UserOffices",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        OfficeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.OfficeId })
                .ForeignKey("dbo.UserDetails", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Offices", t => t.OfficeId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OfficeId);
            
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
            DropForeignKey("dbo.Audiences", "VisitType_VisitTypeId", "dbo.VisitTypes");
            DropForeignKey("dbo.Events", "Office_OfficeId", "dbo.Offices");
            DropForeignKey("dbo.Tasks", "Office_OfficeId", "dbo.Offices");
            DropForeignKey("dbo.Comments", "UserDetail_UserId", "dbo.UserDetails");
            DropForeignKey("dbo.UserDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "UsersDetail_UserId", "dbo.UserDetails");
            DropForeignKey("dbo.UserOffices", "OfficeId", "dbo.Offices");
            DropForeignKey("dbo.UserOffices", "UserId", "dbo.UserDetails");
            DropForeignKey("dbo.UserConventions", "ConventionId", "dbo.Conventions");
            DropForeignKey("dbo.UserConventions", "UserId", "dbo.UserDetails");
            DropForeignKey("dbo.Audiences", "UserDetail_UserId", "dbo.UserDetails");
            DropForeignKey("dbo.Comments", "Task_TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Audiences", "Office_OfficeId", "dbo.Offices");
            DropForeignKey("dbo.Events", "convention_ConventionId", "dbo.Conventions");
            DropForeignKey("dbo.Audiences", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Audiences", "Convention_ConventionId", "dbo.Conventions");
            DropForeignKey("dbo.ConvensionBookings", "AudienceID", "dbo.Audiences");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserOffices", new[] { "OfficeId" });
            DropIndex("dbo.UserOffices", new[] { "UserId" });
            DropIndex("dbo.UserConventions", new[] { "ConventionId" });
            DropIndex("dbo.UserConventions", new[] { "UserId" });
            DropIndex("dbo.UserDetails", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "UserDetail_UserId" });
            DropIndex("dbo.Comments", new[] { "Task_TaskId" });
            DropIndex("dbo.Tasks", new[] { "Office_OfficeId" });
            DropIndex("dbo.Tasks", new[] { "UsersDetail_UserId" });
            DropIndex("dbo.Events", new[] { "Office_OfficeId" });
            DropIndex("dbo.Events", new[] { "convention_ConventionId" });
            DropIndex("dbo.ConvensionBookings", new[] { "AudienceID" });
            DropIndex("dbo.Audiences", new[] { "VisitType_VisitTypeId" });
            DropIndex("dbo.Audiences", new[] { "UserDetail_UserId" });
            DropIndex("dbo.Audiences", new[] { "Office_OfficeId" });
            DropIndex("dbo.Audiences", new[] { "Event_EventId" });
            DropIndex("dbo.Audiences", new[] { "Convention_ConventionId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserOffices");
            DropTable("dbo.UserConventions");
            DropTable("dbo.VisitTypes");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.UserDetails");
            DropTable("dbo.Comments");
            DropTable("dbo.Tasks");
            DropTable("dbo.Offices");
            DropTable("dbo.Events");
            DropTable("dbo.Conventions");
            DropTable("dbo.ConvensionBookings");
            DropTable("dbo.Audiences");
        }
    }
}
