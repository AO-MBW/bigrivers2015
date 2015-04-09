﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// Via OData
//using Bigrivers.Client.DAL.Bigrivers.Server.Model;
//using Bigrivers.Client.DAL.Default;

// Via Direct Reference
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.WebApplication.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController() : base()
        {
            // when homecontroller is instantiated, call BaseController constructor which will fill ViewBag.MenuItemsHTML
        }


        //Create AccessLayer with OData Uri from the App.Config
        //static readonly Container AccessLayer = new Container(new Uri(ConfigurationManager.AppSettings["WebserviceUri"]));

        //Create AccessLayer with direct reference to Server.Data
        //static readonly BigriversDb AccessLayer = new BigriversDb();

        public ActionResult Index()
        {
            return RedirectToAction("Events");
            //return View();
        }

        public ActionResult Events(int? id)
        {
            if (id != null) return Event(id.Value);

            ViewBag.EventList = AccessLayer.Events
                .Include(e => e.Location)
                .Where(e => e.Status)
                .ToList();

            return View("Events");
        }

        private ActionResult Event(int id)
        {
            Event currentEvent = AccessLayer.Events
                .Where(e => e.Id == id)
                .SingleOrDefault();

            ViewBag.CurrentEvent = currentEvent;

            if (currentEvent == null) return Redirect(Request.UrlReferrer.ToString());

            ViewBag.PerformancesList = AccessLayer.Performances
                .Include(p => p.Artist)
                .Where(p => p.Event.Id == currentEvent.Id)
                .ToList();

            return View("Event");
        }

        public ActionResult Performances(int? id)
        {
            if (id != null) return Performance(id.Value);

            ViewBag.PerformancesList = AccessLayer.Performances
                .Where(p => p.Status)
                .OrderBy(p => p.Start)
                .ToList();

            return View("Performances");
        }

        private ActionResult Performance(int id)
        {
            ViewBag.CurrentPerformance = AccessLayer.Performances
                .Where(p => p.Id == id && p.Status)
                .SingleOrDefault();

            if (ViewBag.CurrentPerformance == null) return RedirectToAction("Performances");

            return View("Performance");
        }

        public ActionResult Genres(int? id)
        {
            if (id != null) return Genre(id.Value);

            ViewBag.GenresList = AccessLayer.Genres.ToList();

            return View("Genres");
        }
        private ActionResult Genre(int id)
        {
            ViewBag.CurrentGenre = AccessLayer.Genres
                .Where(g => g.Id == id)
                .SingleOrDefault();

            if (ViewBag.CurrentGenre == null) return RedirectToAction("Genres");

            return View("Genre");
        }

        public ActionResult Artists(int? id)
        {
            if (id != null) return Artist(id.Value);

            ViewBag.ArtistsList = AccessLayer.Artists
                .Where(a => a.Status)
                .ToList();

            return View("Artists");
        }

        private ActionResult Artist(int id)
        {
            ViewBag.CurrentArtist = AccessLayer.Artists
                .Where(a => a.Id == id)
                .SingleOrDefault();

            if (ViewBag.CurrentArtist == null) return RedirectToAction("Artists");

            return View("Artist");
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}