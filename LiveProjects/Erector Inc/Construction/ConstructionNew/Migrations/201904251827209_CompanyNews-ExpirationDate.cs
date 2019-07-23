namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyNewsExpirationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyNews", "ExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyNews", "ExpirationDate");
        }
    }
}
