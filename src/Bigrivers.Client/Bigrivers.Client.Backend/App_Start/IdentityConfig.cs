using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Bigrivers.Client.Backend
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class BigriversStaffManager : UserManager<StaffMember>
    {
        public BigriversStaffManager(IUserStore<StaffMember> store)
            : base(store)
        {
        }

        public static BigriversStaffManager Create(IdentityFactoryOptions<BigriversStaffManager> options, IOwinContext context) 
        {
            var manager = new BigriversStaffManager(new UserStore<StaffMember>(context.Get<BigriversDb>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<StaffMember>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<StaffMember>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class BigriversStaffSignInManager : SignInManager<StaffMember, string>
    {
        public BigriversStaffSignInManager(BigriversStaffManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(StaffMember user)
        {
            return user.GenerateUserIdentityAsync((BigriversStaffManager)UserManager);
        }

        public static BigriversStaffSignInManager Create(IdentityFactoryOptions<BigriversStaffSignInManager> options, IOwinContext context)
        {
            return new BigriversStaffSignInManager(context.GetUserManager<BigriversStaffManager>(), context.Authentication);
        }
    }
}
