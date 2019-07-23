namespace ConstructionNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSenderDataTypeInChatMessage : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ChatMessages", name: "Sender_Id", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.ChatMessages", name: "IX_Sender_Id", newName: "IX_ApplicationUser_Id");
            AddColumn("dbo.ChatMessages", "Sender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatMessages", "Sender");
            RenameIndex(table: "dbo.ChatMessages", name: "IX_ApplicationUser_Id", newName: "IX_Sender_Id");
            RenameColumn(table: "dbo.ChatMessages", name: "ApplicationUser_Id", newName: "Sender_Id");
        }
    }
}
