namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyJobModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "ShiftTimes_ShiftTimeId", c => c.Guid());
            CreateIndex("dbo.Jobs", "ShiftTimes_ShiftTimeId");
            AddForeignKey("dbo.Jobs", "ShiftTimes_ShiftTimeId", "dbo.ShiftTimes", "ShiftTimeId");
            DropColumn("dbo.Jobs", "ShiftTimes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "ShiftTimes", c => c.String());
            DropForeignKey("dbo.Jobs", "ShiftTimes_ShiftTimeId", "dbo.ShiftTimes");
            DropIndex("dbo.Jobs", new[] { "ShiftTimes_ShiftTimeId" });
            DropColumn("dbo.Jobs", "ShiftTimes_ShiftTimeId");
        }
    }
}
