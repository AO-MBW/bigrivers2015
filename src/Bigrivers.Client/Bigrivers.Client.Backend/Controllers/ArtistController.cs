using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

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
            try
            {
                ViewBag.ListArtists = db.Artists
                    .Where(a => a.Status)
                    .ToList();
            }
            catch (Exception e)
            {
                ViewBag.errormsg = e.Message;
            }

            return View();
        }

        // GET: Artist/Details/5
        // TODO: Doesn't work
        private ActionResult Details(int id)
        {
            // Find single artist
            ViewBag.SingleArtist = db.Artists
                .Where(a => a.Id == id)
                .SingleOrDefault();

            // Send to Manage view if artist is not found
            if (ViewBag.SingleArtist == null) return RedirectToAction("Manage");

            return View();
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Artist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
