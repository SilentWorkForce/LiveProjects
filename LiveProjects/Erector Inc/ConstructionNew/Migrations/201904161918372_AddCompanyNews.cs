namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyNews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyNews",
                c => new
                    {
                        DateStamp = c.DateTime(nullable: false),
                        Title = c.String(),
                        NewsItem = c.String(),
                    })
                .PrimaryKey(t => t.DateStamp);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CompanyNews");
        }
    }
}
