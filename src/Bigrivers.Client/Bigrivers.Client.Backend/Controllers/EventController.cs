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
    public class EventController : Controller
    {
        // TODO: Create BaseController class for BigRiversDb
        private readonly BigriversDb _db = new BigriversDb();

        // GET: Event/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Event/
        public ActionResult Manage()
        {
            // List all events and return view
            ViewBag.listEvents = _db.Events.Where(m => !m.Deleted).ToList();

            ViewBag.Title = "Evenementen";
            return View("Manage");
        }

        // GET: Event/New
        public ActionResult New()
        {
            var viewModel = new EventViewModel
            {
                WebsiteStatus = true,
                YoutubeChannelStatus = true,
                FacebookStatus = true,
                TwitterStatus = true,
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(1),
                TicketRequired = false,
                Price = 0.00m,
                Status = true
            };
            ViewBag.Title = "Nieuw Evenement";
            return View("Edit", viewModel);
        }

        // POST: Event/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(EventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw Evenement";
                return View("Edit", viewModel);
            }

            var singleEvent = new Event
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                ShortDescription = viewModel.ShortDescription,
                FrontpageLogo = viewModel.FrontpageLogo,
                BackgroundImage = viewModel.BackgroundImage,
                WebsiteStatus = viewModel.WebsiteStatus,
                YoutubeChannelStatus = viewModel.YoutubeChannelStatus,
                FacebookStatus = viewModel.FacebookStatus,
                TwitterStatus = viewModel.TwitterStatus,
                Start = viewModel.Start,
                End = viewModel.End,
                TicketRequired = viewModel.TicketRequired,
                Price = viewModel.Price ?? 0.00m,
                Status = viewModel.Status
            };

            _db.Events.Add(singleEvent);
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");

            // Find single artist
            var singleEvent = _db.Events.Find(id);

            // Send to Manage view if event is not found
            if (singleEvent == null || singleEvent.Deleted) return RedirectToAction("Manage");

            var model = new EventViewModel
            {
                Title = singleEvent.Title,
                Description = singleEvent.Description,
                ShortDescription = singleEvent.ShortDescription,
                FrontpageLogo = singleEvent.FrontpageLogo,
                BackgroundImage = singleEvent.BackgroundImage,
                WebsiteStatus = singleEvent.WebsiteStatus,
                YoutubeChannelStatus = singleEvent.YoutubeChannelStatus,
                FacebookStatus = singleEvent.FacebookStatus,
                TwitterStatus = singleEvent.TwitterStatus,
                Start = singleEvent.Start.DateTime,
                End = singleEvent.End.DateTime,
                TicketRequired = singleEvent.TicketRequired,
                Price = singleEvent.Price,
                Status = singleEvent.Status
            };

            ViewBag.Title = "Bewerk Evenement";
            return View(model);
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EventViewModel viewModel)
        {
            var singleEvent = _db.Events.Find(id);
            singleEvent.Title = viewModel.Title;
            singleEvent.Description = viewModel.Description;
            singleEvent.ShortDescription = viewModel.ShortDescription;
            singleEvent.FrontpageLogo = viewModel.FrontpageLogo;
            singleEvent.BackgroundImage = viewModel.BackgroundImage;
            singleEvent.WebsiteStatus = viewModel.WebsiteStatus;
            singleEvent.YoutubeChannelStatus = viewModel.YoutubeChannelStatus;
            singleEvent.FacebookStatus = viewModel.FacebookStatus;
            singleEvent.TwitterStatus = viewModel.TwitterStatus;
            singleEvent.Start = viewModel.Start;
            singleEvent.End = viewModel.End;
            singleEvent.TicketRequired = viewModel.TicketRequired;
            singleEvent.Price = viewModel.Price ?? singleEvent.Price;
            singleEvent.Status = viewModel.Status;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            // TODO: Add delete logic here
            var singleEvent = _db.Events.Find(id);

            // Send to Manage view if event is not found
            if (singleEvent == null || singleEvent.Deleted) return RedirectToAction("Manage");

            singleEvent.Deleted = true;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Event/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleEvent = _db.Events.Find(id);

            // Send to Manage view if event is not found
            if (singleEvent == null || singleEvent.Deleted) return RedirectToAction("Manage");

            singleEvent.Status = !singleEvent.Status;
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
