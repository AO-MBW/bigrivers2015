using System;
using System.Collections.Generic;
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
            context.Genres.RemoveRange(context.Genres);
            context.Locations.RemoveRange(context.Locations);
            context.Artists.RemoveRange(context.Artists);
            context.Events.RemoveRange(context.Events);
            context.Performances.RemoveRange(context.Performances);
            context.MenuItems.RemoveRange(context.MenuItems);
            context.ButtonItems.RemoveRange(context.ButtonItems);

            //Create Menu Items
            context.MenuItems.Add(new MenuItem
            {
                URL = "http://www.crowdfunding.nl/",
                DisplayName = "Crowdfunding",
                Order = 1,
                Parent = null,
                IsParent = false,
                Status = true
            });
            context.MenuItems.Add(new MenuItem
            {
                URL = "/Home/Events/",
                DisplayName = "Evenementen",
                Order = 2,
                Parent = null,
                IsParent = false,
                Status = true
            });
            context.MenuItems.Add(new MenuItem
            {
                URL = "/Home/Artists/",
                DisplayName = "Artiesten",
                Order = 3,
                Parent = null,
                IsParent = false,
                Status = true
            });
            context.MenuItems.Add(new MenuItem
            {
                URL = "/Home/Performances/",
                DisplayName = "Optredens",
                Order = 4,
                Parent = null,
                IsParent = false,
                Status = true
            });
            context.MenuItems.Add(new MenuItem
            {
                URL = "/Home/Contact/",
                DisplayName = "Contact",
                Order = 5,
                Parent = null,
                IsParent = false,
                Status = true
            });
            context.MenuItems.Add(new MenuItem
            {
                URL = "/Images/br15.jpg",
                DisplayName = "Logo",
                Order = 6,
                Parent = null,
                IsParent = false,
                Status = true
            });

            var bzb = new Artist
            {
                Name = "Band zonder Banaan",
                Description = "De beschrijving van de Band zonder Banaan",
                Website = "http://bandzonderbanaan.nl",
                YoutubeChannel = "https://youtube.com/user/bandzonderbanaan",
                Facebook = "https://www.facebook.com/bandzonderbanaan",
                Twitter = "https://www.twitter.com/bandzonderbanaan",
                Performances = new List<Performance>(),
                Status = true
            };

            var brm = new Event
            {
                Title = "Bigrivers Muziek",
                ShortDescription = "Bigrivers Muziek!",
                Description = "Het enige echte Bigrivers Muziek evenement!",
                Start = new DateTime(2015, 6, 15, 12, 0, 0),
                End = new DateTime(2015, 6, 15, 22, 0, 0),
                Price = 0,
                TicketRequired = false,
                WebsiteStatus = true,
                YoutubeChannelStatus = true,
                FacebookStatus = true,
                TwitterStatus = true,
                Performances = new List<Performance>(),
                Sponsors = new List<Sponsor>(),
                Status = true
            };

            var bzbperf = new Performance
            {
                Description = "De Band zonder Banaan op Bigrivers Muziek!",
                Start = new DateTime(2015, 6, 15, 12, 0, 0),
                End = new DateTime(2015, 6, 15, 22, 0, 0),
                Artist = bzb,
                Event = brm,
                Status = true
            };

            var dvc = new Sponsor
            {
                Name = "Da Vinci College",
                Url = "http://davincicollege.nl",
                Priority = 0,
                Events = new List<Event>(),
                Status = true
            };

            var loc = new Location
            {
                City = "Dordrecht",
                Street = "Slikveld",
                Zipcode = "3311 VT",
                Stagename = "Bigrivers Kantoor",
                Events = new List<Event>(),
                Status = true
            };

            brm.Performances.Add(bzbperf);
            brm.Location = loc;
            brm.Sponsors.Add(dvc);
            bzb.Performances.Add(bzbperf);
            loc.Events.Add(brm);
            dvc.Events.Add(brm);

            context.Artists.Add(bzb);
            context.Events.Add(brm);
            context.Performances.Add(bzbperf);
            context.Sponsors.Add(dvc);
            context.Locations.Add(loc);

            context.SaveChanges();
        }
    }
}