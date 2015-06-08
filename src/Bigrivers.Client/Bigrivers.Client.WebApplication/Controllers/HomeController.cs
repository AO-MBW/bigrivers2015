using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Client.WebApplication.ViewModels;

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
        //Create AccessLayer with OData Uri from the App.Config
        //static readonly Container AccessLayer = new Container(new Uri(ConfigurationManager.AppSettings["WebserviceUri"]));

        //Create AccessLayer with direct reference to Server.Data
        //static readonly BigriversDb AccessLayer = new BigriversDb();

        public ActionResult Index()
        {
            ViewBag.ButtonItems = AccessLayer.ButtonItems.Where(m => m.Status).OrderBy(m => m.Order).ToList();

            ViewBag.WidgetItems = AccessLayer.WidgetItems.Where(m => m.Status).OrderBy(m => m.Order).ToList();

            ViewBag.Sponsors = AccessLayer.Sponsors.Where(m => m.Status).ToList();

            return View("Index");
        }

        public ActionResult Events(int? id)
        {
            if (id != null) return Event(id.Value);

            ViewBag.EventList = AccessLayer.Events
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
            var socialMedia = AccessLayer.SiteInformation.FirstOrDefault();

            var model = new SettingsViewModel
            {
                YoutubeChannel = "#",
                Facebook = "#",
                Twitter = "#"
            };
            char[] split = {'/'};
            if (socialMedia != null)
            {
                model.YoutubeChannel = socialMedia.YoutubeChannel;
                model.Facebook = socialMedia.Facebook;
                model.Twitter = socialMedia.Twitter;
                model.Hashtag = socialMedia.Twitter.Split(split).Last();
            }

            return View(model);
        }

        public ActionResult News(int? id)
        {
            if (id != null) return News(id.Value);

            ViewBag.NewsItemsList = AccessLayer.NewsItems
                .Where(a => a.Status)
                .ToList();

            return View("News");
        }

        private ActionResult News(int id)
        {
            ViewBag.CurrentNews = AccessLayer.NewsItems
                .Where(a => a.Id == id)
                .SingleOrDefault();

            if (ViewBag.CurrentNews == null) return RedirectToAction("News");

            return View("NewsItem");
        }

    }
}