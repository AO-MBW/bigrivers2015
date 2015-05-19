using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Client.Helpers;

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

        public ActionResult Search(string id)
        {
            var search = id;
            ViewBag.listEvents = GetEvents()
                .Where(m => m.Title.Contains(search))
                .ToList();

            ViewBag.Title = "Zoek Evenementen";
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
        public ActionResult New(EventViewModel viewModel, HttpPostedFileBase logo, HttpPostedFileBase background)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw Evenement";
                return View("Edit", viewModel);
            }

            File frontpageLogo;
            if (ImageHelper.IsSize(logo, 200000) && ImageHelper.IsMimes(logo, new[] { "image" }))
            {
                frontpageLogo = ImageHelper.UploadFile(logo, "eventlogo");
            }
            else
            {
                return RedirectToAction("Manage");
            }

            File backgroundImage;
            if (ImageHelper.IsSize(background, 200000) && ImageHelper.IsMimes(background, new[] { "image" }))
            {
                backgroundImage = ImageHelper.UploadFile(background, "eventbg");
            }
            else
            {
                return RedirectToAction("Manage");
            }

            var singleEvent = new Event
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                ShortDescription = viewModel.ShortDescription,
                FrontpageLogo = frontpageLogo.Key,
                BackgroundImage = backgroundImage.Key,
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

            // Only add file to DB if it hasn't been uploaded before
            if (!Db.Files.Any(m => m.Md5 == backgroundImage.Md5)) Db.Files.Add(backgroundImage);
            if (!Db.Files.Any(m => m.Md5 == frontpageLogo.Md5)) Db.Files.Add(frontpageLogo);
            Db.Events.Add(singleEvent);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");
            var singleEvent = Db.Events.Find(id);
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

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EventViewModel viewModel, HttpPostedFileBase logo, HttpPostedFileBase background)
        {
            File frontpageLogo = null;
            if (logo != null)
            {
                if (ImageHelper.IsSize(logo, 200000) && ImageHelper.IsMimes(logo, new[] { "image" }))
                {
                    frontpageLogo = ImageHelper.UploadFile(logo, "eventlogo");
                }
            }

            File backgroundImage = null;
            if (background != null)
            {
                if (ImageHelper.IsSize(background, 200000) && ImageHelper.IsMimes(background, new[] { "image" }))
                {
                    backgroundImage = ImageHelper.UploadFile(background, "eventbg");
                }
            }

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

            if (frontpageLogo != null && !Db.Files.Any(m => m.Md5 == frontpageLogo.Md5)) singleEvent.FrontpageLogo = frontpageLogo.Key;
            if (backgroundImage != null && !Db.Files.Any(m => m.Md5 == backgroundImage.Md5)) singleEvent.BackgroundImage = backgroundImage.Key;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singleEvent = Db.Events.Find(id);
            if (singleEvent == null || singleEvent.Deleted) return RedirectToAction("Manage");

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
            if (id == null) return RedirectToAction("Manage");
            var singleEvent = Db.Events.Find(id);
            if (singleEvent == null || singleEvent.Deleted) return RedirectToAction("Manage");

            singleEvent.Status = !singleEvent.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Event> GetEvents(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Events : Db.Events.Where(a => !a.Deleted);
        }
    }
}
