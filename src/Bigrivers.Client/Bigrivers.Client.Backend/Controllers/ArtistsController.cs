using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Helpers;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class ArtistsController : BaseController
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

        private static string FileUploadLocation { get { return Helpers.FileUploadLocation.Artist; } }

        // GET: Artist/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Artist/
        public ActionResult Manage()
        {
            var artists = GetArtists();

            var model = artists
                .Where(m => m.Status)
                .ToList();
            model.AddRange(artists.Where(m => !m.Status));

            ViewBag.Title = "Artiesten";
            return View(model);
        }

        // GET: Artist/New
        public ActionResult New()
        {
            var model = new ArtistViewModel
            {
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList()
                },
                Status = true
            };

            ViewBag.Title = "Nieuwe Artiest";
            return View("Edit", model);
        }

        // POST: Artist/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ArtistViewModel model)
        {
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList();
            // Run over a validator to add custom model errors
            foreach (var error in FileValidator.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuwe Artiest";
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

            var singleArtist = new Artist
            {
                Name = model.Name,
                Description = model.Description,
                Avatar = photoEntity != null ? Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key) : null,
                Website = model.Website,
                YoutubeChannel = model.YoutubeChannel,
                Facebook = model.Facebook,
                Twitter = model.Twitter,
                EditedBy = User.Identity.Name,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                Status = model.Status
            };

            Db.Artists.Add(singleArtist);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleArtist = Db.Artists.Find(id);

            var model = new ArtistViewModel
            {
                Name = singleArtist.Name,
                Description = singleArtist.Description,
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    ExistingFile = singleArtist.Avatar,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList()
                },
                Website = singleArtist.Website,
                YoutubeChannel = singleArtist.YoutubeChannel,
                Facebook = singleArtist.Facebook,
                Twitter = singleArtist.Twitter,
                Status = singleArtist.Status
            };

            ViewBag.Title = "Bewerk Artiest";
            return View(model);
        }

        // POST: Artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ArtistViewModel model)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleArtist = Db.Artists.Find(id);

            model.Image.ExistingFile = singleArtist.Avatar;
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileValidator.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuwe Artiest";
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

            singleArtist.Name = model.Name;
            singleArtist.Description = model.Description;
            singleArtist.Website = model.Website;
            singleArtist.YoutubeChannel = model.YoutubeChannel;
            singleArtist.Facebook = model.Facebook;
            singleArtist.Twitter = model.Twitter;
            singleArtist.EditedBy = User.Identity.Name;
            singleArtist.Edited = DateTime.Now;
            singleArtist.Status = model.Status;
            if (photoEntity != null) singleArtist.Avatar = Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Artist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleArtist = Db.Artists.Find(id);

            // Set inactive before deletion so there's no need to explicitly check 'deleted' on frontend
            singleArtist.Status = false;
            singleArtist.Deleted = true;

            // Remove references to artist in other tables
            foreach (var p in singleArtist.Performances)
            {
                p.Artist = null;
            }
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleArtist = Db.Artists.Find(id);

            singleArtist.Status = !singleArtist.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Artist> GetArtists(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Artists : Db.Artists.Where(a => !a.Deleted);
        }

        // Verify if an Artist Id is supplied and corresponds to an existing Artist
        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleArtist = Db.Artists.Find(id);
            return singleArtist != null && !singleArtist.Deleted;
        }
    }
}
