namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchedules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleId = c.Guid(nullable: false),
                        ShiftTime = c.DateTime(nullable: false),
                        WorkType = c.Int(nullable: false),
                        Note = c.String(),
                        JobId_JobId = c.Guid(),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ScheduleId)
                .ForeignKey("dbo.Jobs", t => t.JobId_JobId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.JobId_JobId)
                .Index(t => t.UserId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Schedules", "JobId_JobId", "dbo.Jobs");
            DropIndex("dbo.Schedules", new[] { "UserId_Id" });
            DropIndex("dbo.Schedules", new[] { "JobId_JobId" });
            DropTable("dbo.Schedules");
        }
    }
}
