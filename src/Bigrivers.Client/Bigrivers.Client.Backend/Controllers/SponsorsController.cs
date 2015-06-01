using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Helpers;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class SponsorsController : BaseController
    {
        private static FileUploadValidator FileValidator
        {
            get
            {
                return new FileUploadValidator
                {
                    Required = true,
                    MaxByteSize = 2000000,
                    MimeTypes = new[] { "image" },
                    ModelErrors = new FileUploadModelErrors
                    {
                        Required = "Er moet een afbeelding worden geupload",
                        ExceedsMaxByteSize = "De afbeelding mag niet groter zijn dan 2 MB",
                        ForbiddenMime = "Het bestand moet een afbeelding zijn"
                    }
                };
            }
        }

        private static string FileUploadLocation { get { return Helpers.FileUploadLocation.Sponsor; } }

        // GET: Artist/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Artist/
        public ActionResult Manage()
        {
            var sponsors = GetSponsors().ToList();
            var listSponsors = sponsors.Where(m => m.Status)
                .ToList();

            listSponsors.AddRange(sponsors.Where(m => !m.Status).ToList());
            ViewBag.listSponsors = listSponsors;

            ViewBag.Title = "Nieuws";
            return View("Manage");
        }

        // GET: Artist/New
        public ActionResult New()
        {
            var model = new SponsorViewModel
            {
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList()
                },
                Status = true
            };

            ViewBag.Title = "Sponsor toevoegen";
            return View("Edit", model);
        }

        // POST: Artist/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(SponsorViewModel model)
        {
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList();
            // Run over a validator to add custom model errors
            foreach (var error in FileValidator.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Sponsor toevoegen";
                return View("Edit", model);
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
            
            var singleSponsor = new Sponsor
            {
                Name = model.Name,
                Url = model.Url,
                Image = photoEntity != null ? Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key) : null,
                Status = model.Status
            };

            Db.Sponsors.Add(singleSponsor);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);

            var model = new SponsorViewModel
            {
                Name = singleSponsor.Name,
                Url = singleSponsor.Url,
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    ExistingFile = singleSponsor.Image,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList()
                },
                Status = singleSponsor.Status
            };

            ViewBag.Title = "Bewerk Sponsor";
            return View(model);
        }

        // POST: Artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SponsorViewModel model)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);

            model.Image.ExistingFile = singleSponsor.Image;
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileValidator.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Sponsor toevoegen";
                return View("Edit", model);
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

            singleSponsor.Name = model.Name;
            singleSponsor.Url = model.Url;
            singleSponsor.Status = model.Status;
            if (photoEntity != null) singleSponsor.Image = Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Artist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);

            singleSponsor.Status = false;
            singleSponsor.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);

            singleSponsor.Status = !singleSponsor.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Sponsor> GetSponsors(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Sponsors : Db.Sponsors.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleSponsor = Db.Sponsors.Find(id);
            return singleSponsor != null && !singleSponsor.Deleted;
        }
    }
}
