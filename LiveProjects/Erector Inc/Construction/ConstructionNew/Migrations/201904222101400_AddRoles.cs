namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserRole", c => c.String());
            AddColumn("dbo.CreateUserRequests", "UserRoles", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreateUserRequests", "UserRoles");
            DropColumn("dbo.AspNetUsers", "UserRole");
        }
    }
}
