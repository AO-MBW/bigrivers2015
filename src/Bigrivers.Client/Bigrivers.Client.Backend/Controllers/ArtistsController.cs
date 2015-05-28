using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Helpers;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class ArtistsController : BaseController
    {
        // GET: Artist/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Artist/
        public ActionResult Manage()
        {
            ViewBag.listArtists = ListArtistsInOrder();
            ViewBag.Title = "Artiesten";
            return View("Manage");
        }

        // GET: Artist/New
        public ActionResult New()
        {
            var viewModel = new ArtistViewModel
            {
                Avatar = new FileUploadViewModel
                {
                    NewUpload = true
                },
                Status = true
            };

            var fileBase = Db.Files.Where(m => m.Container == "artist").ToList();
            ViewBag.FileBase = fileBase;

            ViewBag.Title = "Nieuwe Artiest";
            return View("Edit", viewModel);
        }

        // POST: Artist/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ArtistViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var fileBase = Db.Files.Where(m => m.Container == "artist").ToList();
                ViewBag.FileBase = fileBase;
                ViewBag.Title = "Nieuwe Artiest";
                return View("Edit", viewModel);
            }

            File photoEntity = null;
            if (viewModel.Avatar.NewUpload)
            {
                // Upload file to Azurestorage if needed
                if (viewModel.Avatar.UploadFile != null)
                {
                    // File has to be < 2MB and an image
                    if (!FileUploadHelper.IsSize(viewModel.Avatar.UploadFile, 2, "mb"))
                    {
                        ModelState.AddModelError("Avatar", "De afbeelding mag niet groter zijn dan 2 MB");
                    }
                    if (!FileUploadHelper.IsMimes(viewModel.Avatar.UploadFile, new[] {"image"}))
                    {
                        ModelState.AddModelError("Avatar", "Het bestand moet een afbeelding zijn");
                    }
                    if (!ModelState.IsValid)
                    {
                        ViewBag.Title = "Nieuwe Artiest";
                        return View("Edit", viewModel);
                    }
                    photoEntity = FileUploadHelper.UploadFile(viewModel.Avatar.UploadFile, "artist");
                }
            }
            else
            {
                photoEntity = Db.Files.Single(m => m.Key == viewModel.Avatar.Key);
            }
            

            var singleArtist = new Artist
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Avatar = Db.Files.Single(m => m.Key == photoEntity.Key),
                Website = viewModel.Website,
                YoutubeChannel = viewModel.YoutubeChannel,
                Facebook = viewModel.Facebook,
                Twitter = viewModel.Twitter,
                Status = viewModel.Status
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
                Avatar = new FileUploadViewModel
                {
                    NewUpload = true,
                    ExistingFile = singleArtist.Avatar
                },
                Website = singleArtist.Website,
                YoutubeChannel = singleArtist.YoutubeChannel,
                Facebook = singleArtist.Facebook,
                Twitter = singleArtist.Twitter,
                Status = singleArtist.Status
            };

            var fileBase = Db.Files.Where(m => m.Container == "artist").ToList();
            ViewBag.FileBase = fileBase;
            ViewBag.Title = "Bewerk Artiest";
            return View(model);
        }

        // POST: Artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ArtistViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var fileBase = Db.Files.Where(m => m.Container == "artist").ToList();
                ViewBag.FileBase = fileBase;
                ViewBag.Title = "Nieuwe Artiest";
                return View("Edit", viewModel);
            }

            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleArtist = Db.Artists.Find(id);

            File photoEntity = null;
            if (viewModel.Avatar.NewUpload)
            {
                // Upload file to Azurestorage if needed
                if (viewModel.Avatar.UploadFile != null)
                {
                    // File has to be < 2MB and an image
                    if (!FileUploadHelper.IsSize(viewModel.Avatar.UploadFile, 200000))
                    {
                        ModelState.AddModelError("Avatar", "De afbeelding mag niet groter zijn dan 2 MB");
                        ViewBag.Title = "Nieuwe Artiest";
                        return View("Edit", viewModel);
                    }
                    if (!FileUploadHelper.IsMimes(viewModel.Avatar.UploadFile, new[] { "image" }))
                    {
                        ModelState.AddModelError("Avatar", "Het bestand moet een afbeelding zijn");
                        ViewBag.Title = "Nieuwe Artiest";
                        return View("Edit", viewModel);
                    }
                    photoEntity = FileUploadHelper.UploadFile(viewModel.Avatar.UploadFile, "artist");
                }
            }
            else
            {
                photoEntity = Db.Files.Single(m => m.Key == viewModel.Avatar.Key);
            }

            singleArtist.Name = viewModel.Name;
            singleArtist.Description = viewModel.Description;
            singleArtist.Website = viewModel.Website;
            singleArtist.YoutubeChannel = viewModel.YoutubeChannel;
            singleArtist.Facebook = viewModel.Facebook;
            singleArtist.Twitter = viewModel.Twitter;
            singleArtist.Status = viewModel.Status;
            if (photoEntity != null) singleArtist.Avatar = Db.Files.Single(m => m.Key == photoEntity.Key);
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

        private List<Artist> ListArtistsInOrder()
        {
            var artists = GetArtists()
                .ToList();
            var listArtists = artists
                .Where(m => m.Status)
                .ToList();
            listArtists.AddRange(artists.Where(m => !m.Status)
                .ToList());
            return listArtists;
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
