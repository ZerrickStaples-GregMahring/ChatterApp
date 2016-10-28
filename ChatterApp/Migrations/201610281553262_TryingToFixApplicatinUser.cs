namespace ChatterApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingToFixApplicatinUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chats", "Message", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Chats", "Message", c => c.String());
        }
    }
}
