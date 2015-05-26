using System;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bigrivers.Client.Backend.Startup))]
namespace Bigrivers.Client.Backend
{
    public class Startup
    {
        public static Func<UserManager<StaffMember>> UserManagerFactory { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(BigriversDb.Create);
            app.CreatePerOwinContext<BigriversStaffManager>(BigriversStaffManager.Create);
            app.CreatePerOwinContext<BigriversStaffSignInManager>(BigriversStaffSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            UserManagerFactory = () =>
            {
                var usermanager = new UserManager<StaffMember>(
                    new UserStore<StaffMember>(new BigriversDb()));
                // allow alphanumeric characters in username
                usermanager.UserValidator = new UserValidator<StaffMember>(usermanager)
                {
                    AllowOnlyAlphanumericUserNames = true
                };

                return usermanager;
            };
        }
    }
}