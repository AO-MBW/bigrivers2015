using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;
using Bigrivers.Client.Helpers;

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
                Status = true
            };

            ViewBag.Title = "Nieuwe Artiest";
            return View("Edit", viewModel);
        }

        // POST: Artist/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ArtistViewModel viewModel, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuwe Artiest";
                return View("Edit", viewModel);
            }

            // Upload file to Azurestorage if needed
            File photoEntity = null;
            if (file != null)
            {
                // File has to be < 2MB and an image
                if (!ImageHelper.IsSize(file, 2, "mb"))
                {
                    ModelState.AddModelError("Avatar", "De afbeelding mag niet groter zijn dan 2 MB");
                }
                if (!ImageHelper.IsMimes(file, new[] { "image" }))
                {
                    ModelState.AddModelError("Avatar", "Het bestand moet een afbeelding zijn");
                }
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = "Nieuwe Artiest";
                    return View("Edit", viewModel);
                }
                photoEntity = ImageHelper.UploadFile(file, "artist");
            }

            var singleArtist = new Artist
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Avatar = Db.Files.Single(m => m.Md5 == photoEntity.Md5 && m.Container == photoEntity.Container),
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
                Avatar = singleArtist.Avatar,
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
        public ActionResult Edit(int id, ArtistViewModel viewModel, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuwe Artiest";
                return View("Edit", viewModel);
            }

            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleArtist = Db.Artists.Find(id);

            // Upload file to Azurestorage if needed
            File photoEntity = null;
            if (file != null)
            {
                // File has to be < 2MB and an image
                if (!ImageHelper.IsSize(file, 200000))
                {
                    ModelState.AddModelError("Avatar", "De afbeelding mag niet groter zijn dan 2 MB");
                    ViewBag.Title = "Nieuwe Artiest";
                    return View("Edit", viewModel);
                }
                if (!ImageHelper.IsMimes(file, new[] { "image" }))
                {
                    ModelState.AddModelError("Avatar", "Het bestand moet een afbeelding zijn");
                    ViewBag.Title = "Nieuwe Artiest";
                    return View("Edit", viewModel);
                }
                photoEntity = ImageHelper.UploadFile(file, "artist");
            }

            singleArtist.Name = viewModel.Name;
            singleArtist.Description = viewModel.Description;
            singleArtist.Website = viewModel.Website;
            singleArtist.YoutubeChannel = viewModel.YoutubeChannel;
            singleArtist.Facebook = viewModel.Facebook;
            singleArtist.Twitter = viewModel.Twitter;
            singleArtist.Status = viewModel.Status;
            if (photoEntity != null && !Db.Files.Any(m => m.Md5 == photoEntity.Md5)) singleArtist.Avatar = photoEntity;
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
