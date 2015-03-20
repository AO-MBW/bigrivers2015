namespace Bigrivers.Server.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimestoDateTimeOffsets : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Performances", "Start", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Performances", "End", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Events", "Start", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Events", "End", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "End", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Events", "Start", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Performances", "End", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Performances", "Start", c => c.DateTime(nullable: false));
        }
    }
}
