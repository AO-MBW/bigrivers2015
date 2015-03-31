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
            ViewBag.listEvents = _db.Events.ToList();

            ViewBag.Title = "Evenementen";
            return View("Manage");
        }

        // GET: Event/New
        public ActionResult New()
        {
            var viewModel = new EventViewModel
            {
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
        public ActionResult Edit(int id)
        {
            // Find single artist
            var singleEvent = _db.Events.Find(id);

            // Send to Manage view if artist is not found
            if (singleEvent == null) return RedirectToAction("Manage");

            var model = new EventViewModel
            {
                Title = singleEvent.Title,
                Description = singleEvent.Description,
                ShortDescription = singleEvent.ShortDescription,
                FrontpageLogo = singleEvent.FrontpageLogo,
                BackgroundImage = singleEvent.BackgroundImage,
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
            singleEvent.Start = viewModel.Start;
            singleEvent.End = viewModel.End;
            singleEvent.TicketRequired = viewModel.TicketRequired;
            singleEvent.Price = viewModel.Price ?? singleEvent.Price;
            singleEvent.Status = viewModel.Status;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Event/Delete/5
        public ActionResult Delete(int id)
        {
            // TODO: Add delete logic here
            var singleEvent = _db.Events.Find(id);

            _db.Events.Remove(singleEvent);
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Event/SwitchStatus/5
        public ActionResult SwitchStatus(int id)
        {
            var singleEvent = _db.Events.Find(id);

            singleEvent.Status = !singleEvent.Status;
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
