namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateShiftTimes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "ShiftTimes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "ShiftTimes");
        }
    }
}
