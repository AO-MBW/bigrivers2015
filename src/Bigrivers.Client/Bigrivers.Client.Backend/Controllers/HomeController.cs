using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult Settings()
        {
            // Find single artist
            var settings = Db.Links.FirstOrDefault();

            var model = new SettingsViewModel
            {
                YoutubeChannel = "",
                Facebook = "",
                Twitter = ""
            };

            if (settings != null)
            {
                model.YoutubeChannel = settings.YoutubeChannel;
                model.Facebook = settings.Facebook;
                model.Twitter = settings.Twitter;
            }

            ViewBag.Title = "Site Instellingen";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingsViewModel viewModel)
        {
            var settings = Db.Links.FirstOrDefault();
            if (settings != null)
            {
                settings.YoutubeChannel = viewModel.YoutubeChannel;
                settings.Facebook = viewModel.Facebook;
                settings.Twitter = viewModel.Twitter;
            }
            else
            {
                var firstSettings = new Link
                {
                    YoutubeChannel = viewModel.YoutubeChannel,
                    Facebook = viewModel.Facebook,
                    Twitter = viewModel.Twitter
                };
                Db.Links.Add(firstSettings);
            }
            Db.SaveChanges();

            return RedirectToAction("Settings");
        }

    }
}