namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyJobClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "State", c => c.Int(nullable: false));
            DropColumn("dbo.Jobs", "StartDate");
            DropColumn("dbo.Jobs", "EndDate");
            DropColumn("dbo.Jobs", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Jobs", "EndDate", c => c.DateTime());
            AddColumn("dbo.Jobs", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Jobs", "State", c => c.String());
        }
    }
}
