namespace ManagementPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reconstructionTheTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        ChatMessageId = c.Guid(nullable: false),
                        Sender = c.String(),
                        Date = c.DateTime(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.ChatMessageId);
            
            CreateTable(
                "dbo.CompanyNews",
                c => new
                    {
                        DateStamp = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        NewsItem = c.String(),
                        ExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DateStamp);
            
            CreateTable(
                "dbo.CreateUserRequests",
                c => new
                    {
                        UserCreationRequestId = c.Guid(nullable: false),
                        UserName = c.String(),
                        ConfirmationCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserCreationRequestId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        WorkType = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        UserRole = c.String(),
                        Suspended = c.Boolean(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleId = c.Guid(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Job_Id = c.String(maxLength: 128),
                        Person_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ScheduleId)
                .ForeignKey("dbo.Jobs", t => t.Job_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Person_Id)
                .Index(t => t.Job_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ShiftTimes",
                c => new
                    {
                        ShiftTimeId = c.Guid(nullable: false),
                        Monday = c.String(),
                        Tuesday = c.String(),
                        Wednesday = c.String(),
                        Thursday = c.String(),
                        Friday = c.String(),
                        Saturday = c.String(),
                        Sunday = c.String(),
                        Default = c.String(),
                    })
                .PrimaryKey(t => t.ShiftTimeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "Person_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Schedules", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Schedules", new[] { "Person_Id" });
            DropIndex("dbo.Schedules", new[] { "Job_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.ShiftTimes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Schedules");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Jobs");
            DropTable("dbo.CreateUserRequests");
            DropTable("dbo.CompanyNews");
            DropTable("dbo.ChatMessages");
        }
    }
}
