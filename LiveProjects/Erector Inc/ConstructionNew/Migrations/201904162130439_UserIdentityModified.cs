namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdentityModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LName");
            DropColumn("dbo.AspNetUsers", "FName");
        }
    }
}
