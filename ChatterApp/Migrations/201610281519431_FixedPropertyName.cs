namespace ChatterApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedPropertyName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "DatePosted", c => c.DateTime(nullable: false));
            DropColumn("dbo.Chats", "Timestamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chats", "Timestamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.Chats", "DatePosted");
        }
    }
}
