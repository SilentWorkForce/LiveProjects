namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchedule2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "EndDate", c => c.DateTime());
            DropColumn("dbo.Schedules", "Date");
            DropColumn("dbo.Schedules", "WorkType");
            DropColumn("dbo.Schedules", "Note");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "Note", c => c.String());
            AddColumn("dbo.Schedules", "WorkType", c => c.Int(nullable: false));
            AddColumn("dbo.Schedules", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "EndDate");
            DropColumn("dbo.Schedules", "StartDate");
        }
    }
}
