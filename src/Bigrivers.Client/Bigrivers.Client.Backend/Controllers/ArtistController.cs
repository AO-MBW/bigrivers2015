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

        // GET: Artist
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Artist/Manage/5
        public ActionResult Manage(int? id)
        {
            // If artist id is specified, return artist details view
            if (id != null) return Details(id.Value);

            // List all artists and return view
            ViewBag.ListArtists = db.Artists
                .Where(a => a.Status)
                .ToList();

            return View("Manage");
        }

        // GET: Artist/Details/5
        // TODO: Doesn't work
        private ActionResult Details(int id)
        {
            // Find single artist
            var SingleArtist = db.Artists
                .Where(a => a.Id == id)
                .SingleOrDefault();


            // Send to Manage view if artist is not found
            if (SingleArtist == null) return RedirectToAction("Manage");

            ViewBag.SingleArtist = SingleArtist;

            return View("Details");
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArtistViewModel viewModel)
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
            var SingleArtist =
                (from c in db.Artists
                 where c.Id == id
                 select c).First();


            // Send to Manage view if artist is not found
            if (SingleArtist == null) return RedirectToAction("Manage");

            ViewBag.SingleArtist = SingleArtist;

            return View();
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

        // GET: Artist/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Artist/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
