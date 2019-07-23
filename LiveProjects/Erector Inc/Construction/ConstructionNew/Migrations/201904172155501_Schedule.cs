namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Schedule : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Schedules", name: "JobId_JobId", newName: "Job_JobId");
            RenameColumn(table: "dbo.Schedules", name: "UserId_Id", newName: "Person_Id");
            RenameIndex(table: "dbo.Schedules", name: "IX_JobId_JobId", newName: "IX_Job_JobId");
            RenameIndex(table: "dbo.Schedules", name: "IX_UserId_Id", newName: "IX_Person_Id");
            AddColumn("dbo.Schedules", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "ShiftTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "ShiftTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "Date");
            RenameIndex(table: "dbo.Schedules", name: "IX_Person_Id", newName: "IX_UserId_Id");
            RenameIndex(table: "dbo.Schedules", name: "IX_Job_JobId", newName: "IX_JobId_JobId");
            RenameColumn(table: "dbo.Schedules", name: "Person_Id", newName: "UserId_Id");
            RenameColumn(table: "dbo.Schedules", name: "Job_JobId", newName: "JobId_JobId");
        }
    }
}
