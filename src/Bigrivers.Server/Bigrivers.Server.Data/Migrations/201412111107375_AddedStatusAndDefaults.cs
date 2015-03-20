namespace Bigrivers.Server.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddedStatusAndDefaults : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Locations", "Status", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Locations", "Status");
            DropColumn("dbo.Artists", "Status");
        }
    }
}
