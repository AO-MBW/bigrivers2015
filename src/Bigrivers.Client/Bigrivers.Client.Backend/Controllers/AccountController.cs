using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Bigrivers.Client.Backend.Controllers
{
    public class AccountController : BaseController
    {
        #region First stuff
        private BigriversStaffSignInManager _signInManager;
        private BigriversStaffManager _userManager;

        public AccountController()
        {
        }

        public AccountController(BigriversStaffManager userManager, BigriversStaffSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public BigriversStaffSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<BigriversStaffSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public BigriversStaffManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<BigriversStaffManager>(); }
            private set { _userManager = value; }
        }
        #endregion

        public ActionResult Manage()
        {
            if (!UserIsManager) return RedirectToAction("Index", "Home");

            var users = UserManager.Users.ToList().Select(user => new UserViewModel
            {
                Id = user.Id, UserName = user.UserName, Role = UserManager.GetRoles(user.Id).FirstOrDefault()
            }).ToList();

            return View(users);
        }

        #region User Editing
        [AllowAnonymous]
        public ActionResult New()
        {
            return Register();
        }

        public ActionResult Register()
        {
            if (!UserIsManager) return RedirectToAction("Index", "Home");

            var model = new RegisterViewModel();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!UserIsManager) return RedirectToAction("Index", "Home");

            var user = new StaffMember
            {
                UserName = model.LoginName,
            };

            var result = UserManager.Create(user, model.Password);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid) return View(model);

            user.Id = UserManager.FindByName(user.UserName).Id;
            await UserManager.AddToRoleAsync(user.Id, model.Role);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit(string id)
        {
            if (id == null) return RedirectToAction("Register");

            if (!UserIsManager) return RedirectToAction("Index", "Home");

            var user = UserManager.Users.Single(m => m.Id == id);
            if (user == null) return RedirectToAction("Register");

            var model = new RegisterViewModel
            {
                LoginName = user.UserName
            };

            return View("Register", model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View("Register", model);
            if (id == null) return RedirectToAction("Register");

            if (!UserIsManager) return RedirectToAction("Index", "Home");

            var user = UserManager.Users.Single(m => m.Id == id);
            if (user == null) return RedirectToAction("Register");

            user.UserName = model.LoginName;
            if (model.Password != null)
            {
                user.PasswordHash = null;
                await UserManager.AddPasswordAsync(id, model.Password);
            }
            // Get role where role Id == Id of first role of user
            var role = UserManager.GetRoles(id).FirstOrDefault();
            UserManager.RemoveFromRole(id, role);
            await UserManager.AddToRoleAsync(id, model.Role);

            var result = await UserManager.UpdateAsync(user);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            if (!ModelState.IsValid) return View("Register", model);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Login / Logout
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View("Login", "~/Views/Shared/_LayoutLogin.cshtml", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
            {
                return View("Login", "~/Views/Shared/_LayoutLogin.cshtml", model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.LoginName, model.Password, isPersistent: false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View("Login", "~/Views/Shared/_LayoutLogin.cshtml", model);
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private bool UserIsManager
        {
            get { return !User.IsInRole("developer") && !User.IsInRole("Bigrivers Admin"); }
        }

        #endregion
    }
}