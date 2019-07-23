namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateUserRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreateUserRequests",
                c => new
                    {
                        UserCreationRequestId = c.Guid(nullable: false),
                        UserName = c.String(),
                        ConfirmationCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserCreationRequestId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CreateUserRequests");
        }
    }
}
