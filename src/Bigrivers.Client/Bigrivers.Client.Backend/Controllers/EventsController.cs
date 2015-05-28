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
            var events = GetEvents().ToList();
            var listEvents = events.Where(m => m.Status)
                .ToList();

            listEvents.AddRange(events.Where(m => !m.Status).ToList());
            ViewBag.listEvents = listEvents;

            ViewBag.Title = "Evenementen";
            return View("Manage");
        }

        // GET: Events/New
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

        // POST: Events/New
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
            Db.Events.Add(singleEvent);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if(!VerifyId(id)) return RedirectToAction("Manage");
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
                Status = singleEvent.Status
            };

            ViewBag.Title = "Bewerk Evenement";
            return View(model);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw Evenement";
                return View("Edit", viewModel);
            }

            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleEvent = Db.Events.Find(id);

            singleEvent.Title = viewModel.Title;
            singleEvent.Description = viewModel.Description;
            singleEvent.ShortDescription = viewModel.ShortDescription;
            singleEvent.WebsiteStatus = viewModel.WebsiteStatus;
            singleEvent.YoutubeChannelStatus = viewModel.YoutubeChannelStatus;
            singleEvent.FacebookStatus = viewModel.FacebookStatus;
            singleEvent.TwitterStatus = viewModel.TwitterStatus;
            singleEvent.Start = viewModel.Start;
            singleEvent.End = viewModel.End;
            singleEvent.TicketRequired = viewModel.TicketRequired;
            singleEvent.Price = viewModel.Price ?? singleEvent.Price;
            singleEvent.Status = viewModel.Status;

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
