namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJob : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Guid(nullable: false),
                        JobTitle = c.String(),
                        Note = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        JobNumber = c.String(),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zipcode = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.JobId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Jobs");
        }
    }
}
