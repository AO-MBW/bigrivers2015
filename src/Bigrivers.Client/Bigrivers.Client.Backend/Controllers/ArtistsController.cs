using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;

namespace Bigrivers.Client.Backend.Controllers
{
    public class ArtistsController : Controller
    {
        // TODO: Create BaseController class for BigRiversDb
        private readonly BigriversDb _db = new BigriversDb();

        // GET: Artist/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Artist/
        public ActionResult Manage()
        {
            // List all artists and return view
            ViewBag.listArtists = _db.Artists.Where(m => !m.Deleted).ToList();

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
        public ActionResult New(ArtistViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuwe Artiest";
                return View("Edit", viewModel);
            }

            //ViewBag.Title = "Resultaat - Example";
            var singleArtist = new Artist
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Avatar = viewModel.Avatar,
                Website = viewModel.Website,
                YoutubeChannel = viewModel.YoutubeChannel,
                Facebook = viewModel.Facebook,
                Twitter = viewModel.Twitter,
                Status = viewModel.Status
            };

            _db.Artists.Add(singleArtist);
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");

            // Find single artist
            var singleArtist = _db.Artists.Find(id);


            // Send to Manage view if artist is not found
            if (singleArtist == null || singleArtist.Deleted) return RedirectToAction("Manage");

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
        public ActionResult Edit(int id, ArtistViewModel viewModel)
        {
            var singleArtist = _db.Artists.Find(id);

            singleArtist.Name = viewModel.Name;
            singleArtist.Description = viewModel.Description;
            singleArtist.Avatar = viewModel.Avatar;
            singleArtist.Website = viewModel.Website;
            singleArtist.YoutubeChannel = viewModel.YoutubeChannel;
            singleArtist.Facebook = viewModel.Facebook;
            singleArtist.Twitter = viewModel.Twitter;
            singleArtist.Status = viewModel.Status;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Artist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleArtist = _db.Artists.Find(id);

            if (singleArtist == null || singleArtist.Deleted) return RedirectToAction("Manage");

            singleArtist.Deleted = true;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleArtist = _db.Artists.Find(id);

            if (singleArtist == null || singleArtist.Deleted) return RedirectToAction("Manage");

            singleArtist.Status = !singleArtist.Status;
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
