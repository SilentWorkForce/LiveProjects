namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateStampToTicksString : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CompanyNews");
            AlterColumn("dbo.CompanyNews", "DateStamp", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.CompanyNews", "DateStamp");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CompanyNews");
            AlterColumn("dbo.CompanyNews", "DateStamp", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.CompanyNews", "DateStamp");
        }
    }
}
