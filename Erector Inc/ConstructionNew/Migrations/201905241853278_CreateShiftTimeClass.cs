namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateShiftTimeClass : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.ShiftTimes");
        }
    }
}
