using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;

namespace Bigrivers.Client.Backend.Controllers
{
    public class EventsController : BaseController
    {
        // GET: Events/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Events/
        public ActionResult Manage()
        {
            var events = GetEvents();
            var model = events
                .Where(m => m.Status)
                .ToList();

            model.AddRange(events
                .Where(m => !m.Status));

            ViewBag.Title = "Evenementen";
            return View("Manage", model);
        }

        // GET: Events/New
        public ActionResult New()
        {
            var model = new EventViewModel
            {
                WebsiteStatus = true,
                YoutubeChannelStatus = true,
                FacebookStatus = true,
                TwitterStatus = true,
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(1),
                TicketRequired = false,
                Price = 0.00m,
                Location = null,
                Status = true
            };
            ViewBag.Title = "Nieuw Evenement";
            return View("Edit", model);
        }

        // POST: Events/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(EventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw Evenement";
                return View("Edit", model);
            }

            var singleEvent = new Event
            {
                Title = model.Title,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                WebsiteStatus = model.WebsiteStatus,
                YoutubeChannelStatus = model.YoutubeChannelStatus,
                FacebookStatus = model.FacebookStatus,
                TwitterStatus = model.TwitterStatus,
                Start = model.Start,
                End = model.End,
                TicketRequired = model.TicketRequired,
                Price = model.Price ?? 0.00m,
                Location = Db.Locations.Single(m => m.Id == model.Location),
                Status = model.Status
            };
            Db.Events.Add(singleEvent);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleEvent = Db.Events.Find(id);

            var model = new EventViewModel
            {
                Title = singleEvent.Title,
                Description = singleEvent.Description,
                ShortDescription = singleEvent.ShortDescription,
                WebsiteStatus = singleEvent.WebsiteStatus,
                YoutubeChannelStatus = singleEvent.YoutubeChannelStatus,
                FacebookStatus = singleEvent.FacebookStatus,
                TwitterStatus = singleEvent.TwitterStatus,
                Start = singleEvent.Start.DateTime,
                End = singleEvent.End.DateTime,
                TicketRequired = singleEvent.TicketRequired,
                Price = singleEvent.Price,
                Location = singleEvent.Location.Id,
                Status = singleEvent.Status
            };

            ViewBag.Title = "Bewerk Evenement";
            return View(model);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw Evenement";
                return View("Edit", model);
            }

            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleEvent = Db.Events.Find(id);

            singleEvent.Title = model.Title;
            singleEvent.Description = model.Description;
            singleEvent.ShortDescription = model.ShortDescription;
            singleEvent.WebsiteStatus = model.WebsiteStatus;
            singleEvent.YoutubeChannelStatus = model.YoutubeChannelStatus;
            singleEvent.FacebookStatus = model.FacebookStatus;
            singleEvent.TwitterStatus = model.TwitterStatus;
            singleEvent.Start = model.Start;
            singleEvent.End = model.End;
            singleEvent.TicketRequired = model.TicketRequired;
            singleEvent.Price = model.Price ?? singleEvent.Price;
            singleEvent.Location = Db.Locations.Single(m => m.Id == model.Location);
            singleEvent.Status = model.Status;

            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleEvent = Db.Events.Find(id);

            foreach (var p in singleEvent.Performances)
            {
                p.Event = null;
            }
            singleEvent.Status = false;
            singleEvent.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Events/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleEvent = Db.Events.Find(id);

            singleEvent.Status = !singleEvent.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Event> GetEvents(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Events : Db.Events.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleEvent = Db.Events.Find(id);
            return singleEvent != null && !singleEvent.Deleted;
        }
    }
}
