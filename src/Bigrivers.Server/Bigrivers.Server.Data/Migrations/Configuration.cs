using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Bigrivers.Server.Model;

namespace Bigrivers.Server.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BigriversDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BigriversDb context)
        {
#if DEBUG
            // Remove all old data from database
            context.Genres.RemoveRange(context.Genres);
            context.Locations.RemoveRange(context.Locations);
            context.Artists.RemoveRange(context.Artists);
            context.Events.RemoveRange(context.Events);
            context.Performances.RemoveRange(context.Performances);
            context.MenuItems.RemoveRange(context.MenuItems);
            context.ButtonItems.RemoveRange(context.ButtonItems);
            context.WidgetItems.RemoveRange(context.WidgetItems);
            context.NewsItems.RemoveRange(context.NewsItems);
            context.Sponsors.RemoveRange(context.Sponsors);
            context.Links.RemoveRange(context.Links);
            context.Files.RemoveRange(context.Files);
#endif
            foreach (var r in context.Roles)
            {
                context.Roles.Remove(r);
            }
            foreach (var u in context.Users)
            {
                context.Users.Remove(u);
            }
            // Save changes so that you don't have to deal with already existing entities
            context.SaveChanges();

            // Add in all standard data
            context.Roles.AddOrUpdate(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
            {
                Name = "developer"
            });
            context.Roles.AddOrUpdate(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
            {
                Name = "Beheerder"
            });
            context.Roles.AddOrUpdate(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
            {
                Name = "Medewerker"
            });

            if (!context.ButtonItems.Any(m => m.Type == ButtonType.NewsWidget))
            {
                context.ButtonItems.Add(new ButtonItem
                {
                    Type = ButtonType.NewsWidget,
                    DisplayName = "NewsWidget",
                    Order = 1,
                    Created = DateTime.Now,
                    Edited = DateTime.Now,
                    EditedBy = "automatic",
                    Deleted = false,
                    Status = false
                });
            }

            if (!context.ButtonItems.Any(m => m.Type == ButtonType.SponsorWidget))
            {
                context.ButtonItems.Add(new ButtonItem
                {
                    Type = ButtonType.SponsorWidget,
                    DisplayName = "SponsorWidget",
                    Order = 2,
                    Created = DateTime.Now,
                    Edited = DateTime.Now,
                    EditedBy = "automatic",
                    Deleted = false,
                    Status = false
                });
            }

            if (!context.Pages.Any(m => m.IsContactPage))
            {
                context.Pages.Add(new Page
                {
                    IsContactPage = true,
                    Title = "Contact",
                    IFrameHeight = 0,
                    Created = DateTime.Now,
                    Edited = DateTime.Now,
                    EditedBy = "automatic",
                    Deleted = false,
                    Status = false
                });
            }

            if (!context.SiteInformation.Any())
            {
                context.SiteInformation.Add(new SiteInformation
                {
                    YoutubeChannel = "https://www.youtube.com/bigriversdordrecht",
                    Facebook = "https://www.facebook.com/bigriversdordrecht",
                    Twitter = "https://www.twitter.com/bigrivers15",
                    Date = null,
                    Image = null
                });
            }
#if DEBUG
            context.MenuItems.Add(new MenuItem
            {
                DisplayName = "Home",
                Target = new Link
                {
                    Type = "internal",
                    InternalType = "Index"
                },
                Created = DateTime.Now,
                Edited = DateTime.Now,
                EditedBy = "automatic",
                Status = true,
                Deleted = false
            });

            context.MenuItems.Add(new MenuItem
            {
                DisplayName = "Evenementen",
                Target = new Link
                {
                    Type = "internal",
                    InternalType = "Events"
                },
                Created = DateTime.Now,
                Edited = DateTime.Now,
                EditedBy = "automatic",
                Status = true,
                Deleted = false
            });

            context.MenuItems.Add(new MenuItem
            {
                DisplayName = "Artiesten",
                Target = new Link
                {
                    Type = "internal",
                    InternalType = "Artists"
                },
                Created = DateTime.Now,
                Edited = DateTime.Now,
                EditedBy = "automatic",
                Status = true,
                Deleted = false
            });
#endif
            context.SaveChanges();
        }
    }
}