using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Helpers;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class HomeController : BaseController
    {
        private static FileUploadValidator FileValidator
        {
            get
            {
                return new FileUploadValidator
                {
                    Required = false,
                    MaxByteSize = 2000000,
                    MimeTypes = new[] { "image" },
                    ModelErrors = new FileUploadModelErrors
                    {
                        ExceedsMaxByteSize = "De afbeelding mag niet groter zijn dan 2 MB",
                        ForbiddenMime = "Het bestand moet een afbeelding zijn"
                    }
                };
            }
        }

        private static string FileUploadLocation { get { return Helpers.FileUploadLocation.SiteLogo; } }

        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult Settings()
        {
            // Find single artist
            var settings = Db.SiteInformation.FirstOrDefault() ?? new SiteInformation
            {
                YoutubeChannel = "",
                Facebook = "",
                Twitter = "",
                Image = null
            };

            var model = new SettingsViewModel
            {
                YoutubeChannel = settings.YoutubeChannel,
                Facebook = settings.Facebook,
                Twitter = settings.Twitter,
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    ExistingFile = settings.Image,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList()
                }
            };

            ViewBag.Title = "Site Instellingen";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingsViewModel model)
        {
            SiteInformation settings;
            if (Db.SiteInformation.Any())
            {
                settings = Db.SiteInformation.First();
            }
            else
            {
                settings = new SiteInformation
                {
                    YoutubeChannel = "",
                    Facebook = "",
                    Twitter = "",
                    Image = null
                };
            }

            model.Image.ExistingFile = settings.Image;
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileValidator.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Site Instellingen";
                return View("Settings", model);
            }

            File photoEntity = null;
            // Either upload file to AzureStorage or use file Key from explorer to get the file
            if (model.Image.NewUpload)
            {
                if (model.Image.UploadFile != null)
                {
                    photoEntity = FileUploadHelper.UploadFile(model.Image.UploadFile, FileUploadLocation);
                }
            }
            else
            {
                if (model.Image.Key != "false")
                {
                    photoEntity = Db.Files.Single(m => m.Key == model.Image.Key);
                }
            }

            
            if (!model.YoutubeChannel.StartsWith("http")) model.YoutubeChannel = string.Format("http://{0}", model.YoutubeChannel);
            if (!model.Facebook.StartsWith("http")) model.Facebook = string.Format("http://{0}", model.Facebook);
            if (!model.Twitter.StartsWith("http")) model.Twitter = string.Format("http://{0}", model.Twitter);
            settings.YoutubeChannel = model.YoutubeChannel;
            settings.Facebook = model.Facebook;
            settings.Twitter = model.Twitter;
            if (photoEntity != null) settings.Image = Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key);

            if (!Db.SiteInformation.Any()) Db.SiteInformation.Add(settings);
            Db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}