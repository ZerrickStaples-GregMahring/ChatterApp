namespace ChatterApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevertApplicationUserProperty : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Chats", name: "UserName_Id", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Chats", name: "IX_UserName_Id", newName: "IX_ApplicationUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Chats", name: "IX_ApplicationUser_Id", newName: "IX_UserName_Id");
            RenameColumn(table: "dbo.Chats", name: "ApplicationUser_Id", newName: "UserName_Id");
        }
    }
}
