namespace ChatterApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserAndFollowersAttempt : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Chats", name: "UserID", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Chats", name: "IX_UserID", newName: "IX_ApplicationUser_Id");
            CreateTable(
                "dbo.Followers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FollowerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.FollowerId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId)
                .Index(t => t.UserId)
                .Index(t => t.FollowerId);
            
            AddColumn("dbo.Chats", "Timestamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.Chats", "DatePosted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chats", "DatePosted", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Followers", "FollowerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Followers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Followers", new[] { "FollowerId" });
            DropIndex("dbo.Followers", new[] { "UserId" });
            DropColumn("dbo.Chats", "Timestamp");
            DropTable("dbo.Followers");
            RenameIndex(table: "dbo.Chats", name: "IX_ApplicationUser_Id", newName: "IX_UserID");
            RenameColumn(table: "dbo.Chats", name: "ApplicationUser_Id", newName: "UserID");
        }
    }
}
