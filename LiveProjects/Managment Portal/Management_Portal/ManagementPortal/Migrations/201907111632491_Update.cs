namespace ManagementPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CompanyNews");
            CreateTable(
                "dbo.EmployeeInfoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EventModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 100),
                        Name = c.String(maxLength: 100),
                        Description = c.String(),
                        DateOrdered = c.DateTime(nullable: false),
                        DateETA = c.DateTime(nullable: false),
                        URL = c.String(maxLength: 1000),
                        Job_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.Job_Id)
                .Index(t => t.Job_Id);
            
            CreateTable(
                "dbo.Jobsites",
                c => new
                    {
                        JobSiteID = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Town = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.JobSiteID);
            
            AddColumn("dbo.CompanyNews", "NewsId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.CompanyNews", "DateStamp", c => c.String());
            AddPrimaryKey("dbo.CompanyNews", "NewsId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventModels", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.EventModels", new[] { "Job_Id" });
            DropPrimaryKey("dbo.CompanyNews");
            AlterColumn("dbo.CompanyNews", "DateStamp", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.CompanyNews", "NewsId");
            DropTable("dbo.Jobsites");
            DropTable("dbo.EventModels");
            DropTable("dbo.EmployeeInfoes");
            AddPrimaryKey("dbo.CompanyNews", "DateStamp");
        }
    }
}
