using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.WebApplication.Models;
using Bigrivers.Client.WebApplication.ViewModels;
using MoreLinq;

namespace Bigrivers.Client.WebApplication.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.ButtonItems = AccessLayer.ButtonItems
                .Where(m => m.Status)
                .OrderBy(m => m.Order)
                .ToList();

            ViewBag.WidgetItems = AccessLayer.WidgetItems
                .Where(m => m.Status)
                .OrderBy(m => m.Order)
                .ToList();

            ViewBag.Sponsors = AccessLayer.Sponsors
                .Where(m => m.Status)
                .ToList();

            return View("Index");
        }

        public ActionResult Events(int? id)
        {
            if (id != null) return Event(id.Value);

            var eventList = AccessLayer.Events
                .Where(e => e.Status)
                .ToList();

            return View("Events", eventList);
        }

        private ActionResult Event(int id)
        {
            var currentEvent = AccessLayer.Events
                .SingleOrDefault(e => e.Id == id);

            if (currentEvent == null) return RedirectToAction("Events");

            var performancesByDate = currentEvent.Performances
                .Where(m => m.Status)
                .DistinctBy(m => m.Start.Day)
                .Select(date => new PerformanceListViewModel
            {
                Date = date.Start.Date,
                Performances = currentEvent.Performances
                .Where(m => m.Status && m.Start.Day == date.Start.Day)
                .ToList()
            }).ToList();

            ViewBag.PerformancesByDate = performancesByDate;

            return View("Event", currentEvent);
        }

        //public ActionResult Performances(int? id)
        //{
        //    if (id != null) return Performance(id.Value);

            

        //    var newsItemsList = AccessLayer.NewsItems
        //        .Where(a => a.Status)
        //        .ToList();

        //    ViewBag.NewsItemsList = newsItemsList;

        //    return View("Performances");
        //}

        //private ActionResult Performance(int id)
        //{
        //    var currentPerformance = AccessLayer.Performances
        //        .SingleOrDefault(p => p.Id == id && p.Status);

        //    if (currentPerformance == null) return RedirectToAction("Performances");

        //    return View("Performance", currentPerformance);
        //}

        //public ActionResult Genres(int? id)
        //{
        //    if (id != null) return Genre(id.Value);

        //    ViewBag.GenresList = AccessLayer.Genres.ToList();

        //    return View("Genres");
        //}
        //private ActionResult Genre(int id)
        //{
        //    ViewBag.CurrentGenre = AccessLayer.Genres
        //        .Where(g => g.Id == id)
        //        .SingleOrDefault();

        //    if (ViewBag.CurrentGenre == null) return RedirectToAction("Genres");

        //    return View("Genre");
        //}

        public ActionResult Artists(int? id)
        {
            if (id != null) return Artist(id.Value);

            var artistsList = AccessLayer.Artists
                .Where(a => a.Status)
                .OrderBy(a => a.Name)
                .ToList();

            return View("Artists", artistsList);
        }

        private ActionResult Artist(int id)
        {
            var currentArtist = AccessLayer.Artists
                .SingleOrDefault(a => a.Id == id);

            if (currentArtist == null) return RedirectToAction("Artists");

            return View("Artist", currentArtist);
        }

        public ActionResult News(int? id)
        {
            if (id != null) return News(id.Value);

            var newsItemsList = AccessLayer.NewsItems
                .Where(a => a.Status && a.Publish < DateTime.Now)
                .OrderBy(m => m.Publish)
                .ToList();

            return View("News", newsItemsList);
        }

        private ActionResult News(int id)
        {
            var newsItemsList = AccessLayer.NewsItems
                .Where(a => a.Status && a.Publish < DateTime.Now)
                .ToList();

            var currentNews = newsItemsList
                .SingleOrDefault(a => a.Id == id);

            if (currentNews == null) return RedirectToAction("News");
            ViewBag.NewsItemsList = newsItemsList;
            return View("NewsItem", currentNews);
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

        public ActionResult Location(int id)
        {
            var location = AccessLayer.Locations
                .SingleOrDefault(a => a.Id == id);

            if (location == null) return RedirectToAction("Index");
            return View(location);
        }

        public ActionResult Page(int id)
        {
            var page = AccessLayer.Pages
                .SingleOrDefault(a => a.Id == id);

            if (page == null) return RedirectToAction("Index");
            return View(page);
        }
    }
}