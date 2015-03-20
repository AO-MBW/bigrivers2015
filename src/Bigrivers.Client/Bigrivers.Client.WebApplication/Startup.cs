using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bigrivers.Client.WebApplication.Startup))]
namespace Bigrivers.Client.WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
