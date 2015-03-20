namespace Bigrivers.Server.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Avatar = c.String(),
                        YoutubeChannel = c.String(),
                        Website = c.String(),
                        Facebook = c.String(),
                        Twitter = c.String(),
                        Genre = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Performances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Artist_Id = c.Int(),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.Artist_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Artist_Id)
                .Index(t => t.Event_Id);

            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ShortDescription = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketRequired = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Location_Id);

            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        Zipcode = c.String(),
                        City = c.String(),
                        Stagename = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NewsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Image = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Sponsors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.String(),
                        Url = c.String(),
                        Priority = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GenreArtists",
                c => new
                    {
                        Genre_Id = c.Int(nullable: false),
                        Artist_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_Id, t.Artist_Id })
                .ForeignKey("dbo.Genres", t => t.Genre_Id, cascadeDelete: true)
                .ForeignKey("dbo.Artists", t => t.Artist_Id, cascadeDelete: true)
                .Index(t => t.Genre_Id)
                .Index(t => t.Artist_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Performances", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Events", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Performances", "Artist_Id", "dbo.Artists");
            DropForeignKey("dbo.GenreArtists", "Artist_Id", "dbo.Artists");
            DropForeignKey("dbo.GenreArtists", "Genre_Id", "dbo.Genres");
            DropIndex("dbo.GenreArtists", new[] { "Artist_Id" });
            DropIndex("dbo.GenreArtists", new[] { "Genre_Id" });
            DropIndex("dbo.Events", new[] { "Location_Id" });
            DropIndex("dbo.Performances", new[] { "Event_Id" });
            DropIndex("dbo.Performances", new[] { "Artist_Id" });
            DropTable("dbo.GenreArtists");
            DropTable("dbo.Sponsors");
            DropTable("dbo.Pages");
            DropTable("dbo.NewsItems");
            DropTable("dbo.Locations");
            DropTable("dbo.Events");
            DropTable("dbo.Performances");
            DropTable("dbo.Genres");
            DropTable("dbo.Artists");
        }
    }
}
