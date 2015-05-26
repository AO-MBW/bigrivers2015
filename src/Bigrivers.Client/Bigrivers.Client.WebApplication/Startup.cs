using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bigrivers.Client.WebApplication.Startup))]
namespace Bigrivers.Client.WebApplication
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}