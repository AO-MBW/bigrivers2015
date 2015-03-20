using Bigrivers.Server.Model;

namespace Bigrivers.Server.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BigriversDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BigriversDb context)
        {
            context.Genres.RemoveRange(context.Genres);
            context.Locations.RemoveRange(context.Locations);
            context.Artists.RemoveRange(context.Artists);
            context.Events.RemoveRange(context.Events);
            context.Performances.RemoveRange(context.Performances);

            // Create Genres
            Genre rock = new Genre
            {
                Name = "Rock",
                Artists = new List<Artist>()
            };
            Genre pop = new Genre
            {
                Name = "Pop",
                Artists = new List<Artist>()
            };
            Genre blues = new Genre
            {
                Name = "Blues",
                Artists = new List<Artist>()
            };
            Genre dance = new Genre
            {
                Name = "Techno",
                Artists = new List<Artist>()
            };
            Genre metal = new Genre
            {
                Name = "Metal",
                Artists = new List<Artist>()
            };
            Genre klassiek = new Genre
            {
                Name = "Klassiek",
                Artists = new List<Artist>()
            };
            Genre nederlands = new Genre
            {
                Name = "Nederlands",
                Artists = new List<Artist>()
            };
            Genre kPop = new Genre
            {
                Name = "K-Pop",
                Artists = new List<Artist>()
            };

            // Create Location
            Location Sterrenburg = new Location
            {
                Street = "Dordtsestraat",
                Zipcode = "1234 CS",
                City = "Dordrecht",
                Stagename = "Sterrenburg Stage",
                Status = true,
                Events = new List<Event>()
            };
            Location LPP = new Location
            {
                Street = "Leerparkpromenade",
                Zipcode = "5643 LP",
                City = "Dordrecht",
                Stagename = "Davinci Stage",
                Status = true,
                Events = new List<Event>()
            };
            Location Haven = new Location
            {
                Street = "Dordtsehaven",
                Zipcode = "5565 CS",
                City = "Dordrecht",
                Stagename = "Haven Stage",
                Status = true,
                Events = new List<Event>()
            };
            Location DordtCentraal = new Location
            {
                Street = "Dordt Centraal",
                Zipcode = "3240 JS",
                City = "Dordrecht",
                Stagename = "Jonkheer Stage",
                Status = true,
                Events = new List<Event>()
            };

            // Create Artists
            Artist Queen = new Artist
            {
                Name = "Queen",
                Description = "Queen is een Engelse rockgroep. De band is opgericht in 1970 in Londen door gitarist Brian May, zanger Freddie Mercury en drummer Roger Taylor, aangevuld met bassist John Deacon in 1971. Met tientallen hits in de jaren 70, 80 en 90, is Queen een van de succesvolste popgroepen in de geschiedenis.",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/queenofficial",
                Website = "http://www.queenonline.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };

            Artist JusBie = new Artist
            {
                Name = "Justin Bieber",
                Description = "Justin Bieber is een Canadees zanger. Hij begon als kleuter met drummen en gitaarspelen en zong voor het eerst in het openbaar toen hij twaalf jaar oud was. Twee jaar later, nadat hij aan de hand van een video op YouTube was ontdekt, tekende hij zijn eerste platencontract, en groeide vervolgens uit tot een tieneridool.",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/JustinBieberVEVO",
                Website = "http://www.justinbiebermusic.com",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };

            Artist BruMar = new Artist
            {
                Name = "Bruno Mars",
                Description = "Peter Hernandez, beter bekend als Bruno Mars, is een Amerikaans zanger, schrijver en muziekproducent.",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/ElektraRecords",
                Website = "http://www.brunomars.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist Psy = new Artist
            {
                Name = "Psy",
                Description = "Psy - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/Psyofficial",
                Website = "http://www.psyofficial.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist Coldplay = new Artist
            {
                Name = "Coldplay",
                Description = "Coldplay - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/ColdplayVEVO",
                Website = "http://www.coldplay.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist NicSim = new Artist
            {
                Name = "Nick & Simon",
                Description = "Nick & Simon - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/NickSimonNL",
                Website = "http://www.nicksimon.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist Jamai = new Artist
            {
                Name = "Jamai",
                Description = "Jamai - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/JamaiNL",
                Website = "http://www.jamainl.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist BluesBros = new Artist
            {
                Name = "Blues Brothers",
                Description = "The Blues Brothers, more formally called The Blues Brothers' Show Band and Revue, are an American blues and rhythm and blues revivalist band founded in 1978 by comedy actors Dan Aykroyd and John Belushi as part of a musical sketch on Saturday Night Live",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/BluesBrothers",
                Website = "http://www.bluesbrothers.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist ArmVBuu = new Artist
            {
                Name = "Armin Van Buuren",
                Description = "Armin van Buuren - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/arminvanbuuren",
                Website = "http://www.arminvanbuuren.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist Bastille = new Artist
            {
                Name = "Bastille",
                Description = "Bastille - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/Bastille",
                Website = "http://www.Bastille.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist FooFight = new Artist
            {
                Name = "Foo Fighters",
                Description = "Foo Fighters - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/FooFighters",
                Website = "http://www.FooFighters.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist NieGeu = new Artist
            {
                Name = "Niels Geusebroek",
                Description = "Niels Geusebroek - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/NielsGeusebroek",
                Website = "http://www.nielsgeusebroek.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist Pitbull = new Artist
            {
                Name = "Pitbull",
                Description = "Pitbull - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/PitbullVEVO",
                Website = "http://www.pitbullmusic.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };
            Artist Script = new Artist
            {
                Name = "The Script",
                Description = "The Script - Beschrijving",
                Avatar = null,
                YoutubeChannel = "https://www.youtube.com/user/Jeroenvdboom",
                Website = "http://www.jeroenboom.com/",
                Facebook = null,
                Twitter = null,
                Status = true,
                Performances = new List<Performance>(),
                Genres = new List<Genre>()
            };

            // Create Events
            Event tgfEvent = new Event
            {
                Title = "Teen Girl Fest",
                Description = "Music for the younger girls, like Justin Bieber and One Direction",
                ShortDescription = "Music for the younger girls",
                Start = DateTime.Now.AddDays(5),
                End = DateTime.Now.AddDays(5).AddHours(3),
                Price = 13.37m,
                TicketRequired = true,
                Status = true,
                Performances = new List<Performance>(),
                Location = Sterrenburg
            };
            Event bigEverythingEvent = new Event
            {
                Title = "BigEverything",
                Description = "All kinds of unrelated music stuffed together! Cause why not?",
                ShortDescription = "All kinds of unrelated music!",
                Start = DateTime.Now.AddDays(4),
                End = DateTime.Now.AddDays(4).AddHours(6),
                Price = 13.37m,
                TicketRequired = true,
                Status = true,
                Performances = new List<Performance>(),
            };
            Event breakingFreeEvent = new Event
            {
                Title = "Breaking Free",
                Description = "All kinds of 60's and 70's music, with a main act from Queen!",
                ShortDescription = "All kinds of 60's and 70's music",
                Start = DateTime.Now.AddDays(2),
                End = DateTime.Now.AddDays(2).AddHours(3),
                Price = 13.37m,
                TicketRequired = true,
                Status = true,
                Performances = new List<Performance>(),
            };
            Event dutchPartyEvent = new Event
            {
                Title = "Dutch Party",
                Description = "Nederlandse artiesten zoals Jeroen vd Boom, Nick & Simon bij elkaar!",
                ShortDescription = "Nederlandse artiesten bij elkaar!",
                Start = DateTime.Now.AddDays(3).AddHours(5),
                End = DateTime.Now.AddDays(3).AddHours(7),
                Price = 13.37m,
                TicketRequired = true,
                Status = true,
                Performances = new List<Performance>(),
            };

            Event liveDanceEvent = new Event
            {
                Title = "Live Dance",
                Description = "Dancing music on the big stage!",
                ShortDescription = "Dancing music!",
                Start = DateTime.Now.AddDays(4).AddHours(7),
                End = DateTime.Now.AddDays(4).AddHours(11),
                Price = 13.37m,
                TicketRequired = true,
                Status = true,
                Performances = new List<Performance>(),
            };


            // Create Performances for Breakingfree
            Performance queenBreakingFreePerformanceP1 = new Performance
            {
                Description = "Queen live at Breaking Free",
                Start = breakingFreeEvent.Start,
                End = breakingFreeEvent.Start.AddHours(1),
                Status = true,
                Artist = Queen,
                Event = breakingFreeEvent
            };
            Performance bluesBrosBreakingFreePerformance = new Performance()
            {
                Description = "Blues Brothers at Breaking Free event",
                Start = queenBreakingFreePerformanceP1.End,
                End = queenBreakingFreePerformanceP1.End.AddHours(0.5),
                Status = true,
                Artist = BluesBros,
                Event = breakingFreeEvent
            };
            Performance queenBreakingFreePerformanceP2 = new Performance
            {
                Description = "Queen live at Breaking Free for the second time the evening",
                Start = bluesBrosBreakingFreePerformance.End,
                End = breakingFreeEvent.End,
                Status = true,
                Artist = Queen,
                Event = breakingFreeEvent
            };

            // Create Performances for TGF
            Performance justinBieberTgfPerformance = new Performance
            {
                Description = "Justin Bieber does stuff",
                Start = tgfEvent.Start,
                End = tgfEvent.Start.AddHours(1.5),
                Status = true,
                Artist = JusBie,
                Event = tgfEvent
            };
            Performance brunoMarsTgfPerformance = new Performance
            {
                Description = "Bruno Mars does stuff",
                Start = justinBieberTgfPerformance.End,
                End = tgfEvent.End,
                Status = true,
                Artist = BruMar,
                Event = tgfEvent
            };

            // Create performances for Dutchparty
            Performance nickEnSimonDutchPartyPerformance = new Performance
            {
                Description = "Nick en Simon performance",
                Start = dutchPartyEvent.Start,
                End = dutchPartyEvent.Start.AddHours(1),
                Status = true,
                Artist = NicSim,
                Event = dutchPartyEvent
            };
            Performance jamaiDutchPartyPerformance = new Performance
            {
                Description = "Jamai performance",
                Start = nickEnSimonDutchPartyPerformance.Start,
                End = nickEnSimonDutchPartyPerformance.Start.AddHours(1),
                Status = true,
                Artist = Jamai,
                Event = dutchPartyEvent
            };

            // Create performances for LiveDance
            Performance nielsGeusebroekLiveDancePerformanceP1 = new Performance
            {
                Description = "Niels Geusebroek performance 1",
                Start = liveDanceEvent.Start,
                End = liveDanceEvent.Start.AddHours(0.5),
                Status = true,
                Artist = NieGeu,
                Event = liveDanceEvent
            };
            Performance pitbullLiveDancePerformance = new Performance
            {
                Description = "Pitbull performance",
                Start = nielsGeusebroekLiveDancePerformanceP1.End,
                End = nielsGeusebroekLiveDancePerformanceP1.End.AddHours(1),
                Status = true,
                Artist = Pitbull,
                Event = liveDanceEvent
            };
            Performance nielsGeusebroekLiveDancePerformanceP2 = new Performance
            {
                Description = "Niels Geusebroek performance 2",
                Start = pitbullLiveDancePerformance.End,
                End = pitbullLiveDancePerformance.End.AddHours(1.5),
                Status = true,
                Artist = NieGeu,
                Event = liveDanceEvent
            };
            Performance arminVanBuurenLiveDancePerformance = new Performance
            {
                Description = "Niels Geusebroek performance",
                Start = nielsGeusebroekLiveDancePerformanceP2.End,
                End = liveDanceEvent.End,
                Status = true,
                Artist = ArmVBuu,
                Event = liveDanceEvent
            };

            // Add genres to artists
            Queen.Genres.Add(rock);
            JusBie.Genres.Add(pop);
            BruMar.Genres.Add(pop);
            BluesBros.Genres.Add(blues);
            Script.Genres.Add(pop);
            NicSim.Genres.Add(nederlands);
            Jamai.Genres.Add(nederlands);
            NieGeu.Genres.Add(dance);
            Pitbull.Genres.Add(dance);
            ArmVBuu.Genres.Add(dance);
            Coldplay.Genres.Add(pop);
            Psy.Genres.Add(kPop);
            FooFight.Genres.Add(metal);
            Bastille.Genres.Add(pop);


            // Add artists to genres
            rock.Artists.Add(Queen);
            pop.Artists.Add(JusBie);
            pop.Artists.Add(BruMar);
            blues.Artists.Add(BluesBros);
            pop.Artists.Add(Script);
            nederlands.Artists.Add(NicSim);
            nederlands.Artists.Add(Jamai);
            dance.Artists.Add(ArmVBuu);
            dance.Artists.Add(Pitbull);
            dance.Artists.Add(NieGeu);
            pop.Artists.Add(Coldplay);
            kPop.Artists.Add(Psy);
            metal.Artists.Add(FooFight);
            pop.Artists.Add(Bastille);

            // Add performances to artists
            Queen.Performances.Add(queenBreakingFreePerformanceP1);
            Queen.Performances.Add(queenBreakingFreePerformanceP2);
            BluesBros.Performances.Add(bluesBrosBreakingFreePerformance);

            JusBie.Performances.Add(justinBieberTgfPerformance);
            BruMar.Performances.Add(brunoMarsTgfPerformance);

            NicSim.Performances.Add(nickEnSimonDutchPartyPerformance);
            Jamai.Performances.Add(jamaiDutchPartyPerformance);

            NieGeu.Performances.Add(nielsGeusebroekLiveDancePerformanceP1);
            NieGeu.Performances.Add(nielsGeusebroekLiveDancePerformanceP2);
            Pitbull.Performances.Add(pitbullLiveDancePerformance);
            ArmVBuu.Performances.Add(arminVanBuurenLiveDancePerformance);

            // Add performances to events
            breakingFreeEvent.Performances.Add(queenBreakingFreePerformanceP1);
            breakingFreeEvent.Performances.Add(bluesBrosBreakingFreePerformance);
            breakingFreeEvent.Performances.Add(queenBreakingFreePerformanceP2);

            tgfEvent.Performances.Add(justinBieberTgfPerformance);
            tgfEvent.Performances.Add(brunoMarsTgfPerformance);
            
            dutchPartyEvent.Performances.Add(nickEnSimonDutchPartyPerformance);
            dutchPartyEvent.Performances.Add(jamaiDutchPartyPerformance);

            liveDanceEvent.Performances.Add(nielsGeusebroekLiveDancePerformanceP1);
            liveDanceEvent.Performances.Add(pitbullLiveDancePerformance);
            liveDanceEvent.Performances.Add(nielsGeusebroekLiveDancePerformanceP2);
            liveDanceEvent.Performances.Add(arminVanBuurenLiveDancePerformance);


            // Add genres to context
            context.Genres.AddOrUpdate(rock);
            context.Genres.AddOrUpdate(pop);
            context.Genres.AddOrUpdate(blues);
            context.Genres.AddOrUpdate(metal);
            context.Genres.AddOrUpdate(dance);
            context.Genres.AddOrUpdate(nederlands);
            context.Genres.AddOrUpdate(kPop);
            context.Genres.AddOrUpdate(klassiek);

            // Add locations to context
            context.Locations.AddOrUpdate(Sterrenburg);
            context.Locations.AddOrUpdate(LPP);
            context.Locations.AddOrUpdate(Haven);
            context.Locations.AddOrUpdate(DordtCentraal);

            // Add artists to context
            context.Artists.AddOrUpdate(Queen);
            context.Artists.AddOrUpdate(JusBie);
            context.Artists.AddOrUpdate(BruMar);
            context.Artists.AddOrUpdate(Psy);
            context.Artists.AddOrUpdate(Coldplay);
            context.Artists.AddOrUpdate(Pitbull);
            context.Artists.AddOrUpdate(NieGeu);
            context.Artists.AddOrUpdate(ArmVBuu);
            context.Artists.AddOrUpdate(BluesBros);
            context.Artists.AddOrUpdate(Script);
            context.Artists.AddOrUpdate(FooFight);
            context.Artists.AddOrUpdate(NicSim);
            context.Artists.AddOrUpdate(Jamai);
            context.Artists.AddOrUpdate(Bastille);

            // Add events to context
            context.Events.AddOrUpdate(tgfEvent);
            context.Events.AddOrUpdate(breakingFreeEvent);
            context.Events.AddOrUpdate(liveDanceEvent);
            context.Events.AddOrUpdate(dutchPartyEvent);
            context.Events.AddOrUpdate(bigEverythingEvent);

            // Add performances to context
            context.Performances.AddOrUpdate(queenBreakingFreePerformanceP1);
            context.Performances.AddOrUpdate(queenBreakingFreePerformanceP2);
            context.Performances.AddOrUpdate(bluesBrosBreakingFreePerformance);
            context.Performances.AddOrUpdate(justinBieberTgfPerformance);
            context.Performances.AddOrUpdate(brunoMarsTgfPerformance);
            context.Performances.AddOrUpdate(nickEnSimonDutchPartyPerformance);
            context.Performances.AddOrUpdate(jamaiDutchPartyPerformance);
            context.Performances.AddOrUpdate(pitbullLiveDancePerformance);
            context.Performances.AddOrUpdate(nielsGeusebroekLiveDancePerformanceP1);
            context.Performances.AddOrUpdate(nielsGeusebroekLiveDancePerformanceP2);
            context.Performances.AddOrUpdate(arminVanBuurenLiveDancePerformance);

            //context.Locations.AddOrUpdate();
            //context.Artists.AddOrUpdate();
            //context.Events.AddOrUpdate();
            //context.Performances.AddOrUpdate();

            context.SaveChanges();
        }
    }
}