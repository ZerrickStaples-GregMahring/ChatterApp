namespace ChatterApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingApplicationUserForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Chats", name: "ApplicationUser_Id", newName: "UserID");
            RenameIndex(table: "dbo.Chats", name: "IX_ApplicationUser_Id", newName: "IX_UserID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Chats", name: "IX_UserID", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Chats", name: "UserID", newName: "ApplicationUser_Id");
        }
    }
}
