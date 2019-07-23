namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddWorkType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WorkType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WorkType");
        }
    }
}
