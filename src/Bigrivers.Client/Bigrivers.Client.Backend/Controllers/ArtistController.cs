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
    public class ArtistController : Controller
    {
        // TODO: Create BaseController class for BigRiversDb
        private readonly BigriversDb db = new BigriversDb();

        // GET: Artist/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Artist/
        public ActionResult Manage()
        {
            // List all artists and return view
            ViewBag.listArtists =
                (from a in db.Artists
                 select a).ToList();

            return View("Manage");
        }

        // GET: Artist/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: Artist/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ArtistViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            //ViewBag.Title = "Resultaat - Example";
            var a = new Artist
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

            db.Artists.Add(a);
            db.SaveChanges();

            ViewBag.ObjectAdded = viewModel;

            return RedirectToAction("Manage");
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int id)
        {
            // Find single artist
            var artist =
                (from a in db.Artists
                 where a.Id == id
                 select a).First();


            // Send to Manage view if artist is not found
            if (artist == null) return RedirectToAction("Manage");

            var model = new ArtistViewModel
            {
                Name = artist.Name,
                Description = artist.Description,
                Avatar = artist.Avatar,
                Website = artist.Website,
                YoutubeChannel = artist.YoutubeChannel,
                Facebook = artist.Facebook,
                Twitter = artist.Twitter,
                Status = artist.Status
            };

            return View(model);
        }

        // POST: Artist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ArtistViewModel viewModel)
        {
            var artist =
                (from c in db.Artists
                 where c.Id == id
                 select c).First();

            artist.Name = viewModel.Name;
            artist.Description = viewModel.Description;
            artist.Avatar = viewModel.Avatar;
            artist.Website = viewModel.Website;
            artist.YoutubeChannel = viewModel.YoutubeChannel;
            artist.Facebook = viewModel.Facebook;
            artist.Twitter = viewModel.Twitter;
            artist.Status = viewModel.Status;
            db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Artist/Delete/5
        public ActionResult Delete(int id)
        {
            // TODO: Add delete logic here
            var artist =
                (from c in db.Artists
                 where c.Id == id
                 select c).First();

            db.Artists.Remove(artist);
            db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/SwitchStatus/5
        public ActionResult SwitchStatus(int id)
        {
            var artist =
            (from a in db.Artists
                where a.Id == id
                select a).First();

            artist.Status = !artist.Status;
            db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
