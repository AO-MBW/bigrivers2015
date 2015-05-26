using System.Data.Entity;
using Bigrivers.Server.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bigrivers.Server.Data
{
    public class BigriversDb : IdentityDbContext<StaffMember>
    {
        // Your context has been configured to use a 'Model' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Bigrivers.Server.Data.Model' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model' 
        // connection string in the application configuration file.
        public BigriversDb()
            : base("name=BigriversContext")
        {
           // this.Configuration.LazyLoadingEnabled = false;
        }

        public static BigriversDb Create()
        {
            return new BigriversDb();
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<NewsItem> NewsItems { get; set; }
        public virtual DbSet<Performance> Performances { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Sponsor> Sponsors { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<ButtonItem> ButtonItems { get; set; }
        public virtual DbSet<SiteInformation> SiteInformation { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Link> Links { get; set; }
    }
}