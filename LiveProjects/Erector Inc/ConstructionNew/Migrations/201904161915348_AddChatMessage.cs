namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChatMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        ChatMessageId = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Message = c.String(),
                        Sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ChatMessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .Index(t => t.Sender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ChatMessages", new[] { "Sender_Id" });
            DropTable("dbo.ChatMessages");
        }
    }
}
