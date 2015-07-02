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
                .Where(m => m.Status)
                .SingleOrDefault(e => e.Id == id);

            if (currentEvent == null) return RedirectToAction("Events");

            var performancesByDate = currentEvent.Performances
                .Where(m => m.Status)
                .DistinctBy(m => m.Start.AddHours(-6).Date)
                .OrderBy(m => m.Start.Date)
                .Select(date => new StagesViewModel
            {
                Date = date.Start.Date,
                Stages = currentEvent.Performances
                .Where(m => m.Start.Date == date.Start.Date && m.Status)
                .DistinctBy(m => m.Location)
                .Select(stage => new PerformancesByLocationViewModel
                {
                    Stage = stage.Location,
                    Performances = currentEvent.Performances
                    .Where(m => m.Location == stage.Location && m.Start.AddHours(-6).Date == date.Start.Date && m.Status)
                    .OrderBy(m => m.Start.DateTime)
                    .ToList()
                }).ToList()
            }).ToList();

            ViewBag.PerformancesByDate = performancesByDate;

            return View("Event", currentEvent);
        }

        public ActionResult performances(int? id)
        {
            var currentEvent = AccessLayer.Events
                .Where(m => m.Status)
                .SingleOrDefault(e => e.Id == id);

            if (currentEvent == null) return RedirectToAction("Events");

            var performancesByDate = currentEvent.Performances
                .Where(m => m.Status)
                .DistinctBy(m => m.Start.AddHours(-6).Date)
                .OrderBy(m => m.Start.Date)
                .Select(date => new StagesViewModel
                {
                    Date = date.Start.Date,
                    Stages = currentEvent.Performances
                    .Where(m => m.Start.Date == date.Start.Date && m.Status)
                    .DistinctBy(m => m.Location)
                    .Select(stage => new PerformancesByLocationViewModel
                    {
                        Stage = stage.Location,
                        Performances = currentEvent.Performances
                        .Where(m => m.Location == stage.Location && m.Start.AddHours(-6).Date == date.Start.Date && m.Status)
                        .OrderBy(m => m.Start.DateTime)
                        .ToList()
                    }).ToList()
                }).ToList();

            ViewBag.PerformancesByDate = performancesByDate;

            return View("Performances", currentEvent);
        }

        //private actionresult performance(int id)
        //{
        //    var currentperformance = accesslayer.performances
        //        .singleordefault(p => p.id == id && p.status);

        //    if (currentperformance == null) return redirecttoaction("performances");

        //    return view("performance", currentperformance);
        //}

        //public actionresult genres(int? id)
        //{
        //    if (id != null) return genre(id.value);

        //    viewbag.genreslist = accesslayer.genres.tolist();

        //    return view("genres");
        //}
        //private actionresult genre(int id)
        //{
        //    viewbag.currentgenre = accesslayer.genres
        //        .where(g => g.id == id)
        //        .singleordefault();

        //    if (viewbag.currentgenre == null) return redirecttoaction("genres");

        //    return view("genre");
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
                .SingleOrDefault(a => a.Status && a.Id == id);

            if (currentArtist == null) return RedirectToAction("Artists");

            return View("Artist", currentArtist);
        }

        public ActionResult News(int? id)
        {
            if (id != null) return News(id.Value);

            var newsItemsList = AccessLayer.NewsItems
                .Where(a => a.Status && a.Publish < DateTime.Now)
                .OrderByDescending(m => m.Publish)
                .ToList();

            return View("News", newsItemsList);
        }

        private ActionResult News(int id)
        {
            var newsItemsList = AccessLayer.NewsItems
                .Where(a => a.Status && a.Publish < DateTime.Now)
                .ToList();

            var currentNews = newsItemsList
                .SingleOrDefault(a => a.Status && a.Id == id);

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

            if (socialMedia != null)
            {
                model.YoutubeChannel = socialMedia.YoutubeChannel;
                model.Facebook = socialMedia.Facebook;
                model.Twitter = socialMedia.Twitter;
            }

            return View(model);
        }

        public ActionResult Location(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var location = AccessLayer.Locations
                .SingleOrDefault(a => a.Status && a.Id == id);

            if (location == null) return RedirectToAction("Index");
            return View(location);
        }

        public ActionResult Page(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var page = AccessLayer.Pages
                .SingleOrDefault(a => a.Status && a.Id == id);

            if (page == null) return RedirectToAction("Index");
            return View(page);
        }
    }
}