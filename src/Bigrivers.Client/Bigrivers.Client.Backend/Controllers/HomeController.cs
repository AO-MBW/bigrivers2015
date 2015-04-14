using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly BigriversDb _db = new BigriversDb();

        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult Settings()
        {
            // Find single artist
            var settings = _db.Links.FirstOrDefault();

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
        public ActionResult Settings(SettingsViewModel viewModel)
        {
            var settings = _db.Links.FirstOrDefault();
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
                _db.Links.Add(firstSettings);
            }
            _db.SaveChanges();

            return RedirectToAction("Settings");
        }

    }
}