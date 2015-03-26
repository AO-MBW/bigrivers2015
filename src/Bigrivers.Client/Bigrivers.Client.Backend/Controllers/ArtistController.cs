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

        // POST: Artist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ArtistViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else
            {
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
                    Status = true
                };

                db.Artists.Add(a);
                db.SaveChanges();

                ViewBag.ObjectAdded = viewModel;

                return RedirectToAction("Manage");
            }
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

            var m = new ArtistViewModel
            {
                Name = artist.Name,
                Description = artist.Description,
                Avatar = artist.Avatar,
                Website = artist.Website,
                YoutubeChannel = artist.YoutubeChannel,
                Facebook = artist.Facebook,
                Twitter = artist.Twitter
            };

            return View(m);
        }

        // POST: Artist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ArtistViewModel viewModel)
        {
            var SingleArtist =
                (from c in db.Artists
                 where c.Id == id
                 select c).First();

            SingleArtist.Name = viewModel.Name;
            SingleArtist.Description = viewModel.Description;
            SingleArtist.Avatar = viewModel.Avatar;
            SingleArtist.Website = viewModel.Website;
            SingleArtist.YoutubeChannel = viewModel.YoutubeChannel;
            SingleArtist.Facebook = viewModel.Facebook;
            SingleArtist.Twitter = viewModel.Twitter;
            db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Artist/Delete/5
        public ActionResult Delete(int id)
        {
            // TODO: Add delete logic here
            var SingleArtist =
                (from c in db.Artists
                 where c.Id == id
                 select c).First();

            db.Artists.Remove(SingleArtist);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Artist/Action/5/Status
        public ActionResult SwitchStatus(int id)
        {
            var artist =
            (from a in db.Artists
                where a.Id == id
                select a).First();

            if (artist.Status)
            {
                artist.Status = false;
            }
            else
            {
                artist.Status = true;
            }

            db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
