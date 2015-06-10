using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;

namespace Bigrivers.Client.Backend.Controllers
{
    public class PerformancesController : BaseController
    {
        // GET: Performances/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Performances/
        public ActionResult Manage()
        {
            var performances = GetPerformances();
            var model = performances
                .Where(m => m.Status)
                .ToList();

            model.AddRange(performances
                .Where(m => !m.Status));

            ViewBag.Title = "Optredens";
            return View(model);
        }

        // GET: Performances/Create
        public ActionResult New()
        {
            var viewModel = new PerformanceViewModel
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(1),
                Status = true,
                Event = null,
                Artist = null
            };

            ViewBag.Title = "Nieuw Optreden";
            return View("Edit", viewModel);
        }

        // POST: Performances/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(PerformanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw Optreden";
                return View("Edit", model);
            }

            var singlePerformance = new Performance
            {
                Description = model.Description,
                Start = model.Start,
                End = model.End,
                EditedBy = User.Identity.Name,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                Status = model.Status,
                Event = Db.Events.Single(m => m.Id == model.Event),
                Artist = Db.Artists.Single(m => m.Id == model.Artist),
                Location = Db.Locations.Single(m => m.Id == model.Location)
            };

            Db.Performances.Add(singlePerformance);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Performances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singlePerformance = Db.Performances.Find(id);

            var viewModel = new PerformanceViewModel
            {
                Description = singlePerformance.Description,
                Start = singlePerformance.Start.DateTime,
                End = singlePerformance.End.DateTime,
                Status = singlePerformance.Status,
                Event = singlePerformance.Event.Id,
                Artist = singlePerformance.Artist.Id,
                Location = singlePerformance.Location.Id
            };

            // Set all active parents into new list first

            ViewBag.Title = "Bewerk Optreden";
            return View(viewModel);
        }

        // POST: Performances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PerformanceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Bewerk Optreden";
                return View("Edit", viewModel);
            }

            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singlePerformance = Db.Performances.Find(id);

            singlePerformance.Description = viewModel.Description;
            singlePerformance.Start = viewModel.Start;
            singlePerformance.End = viewModel.End;
            singlePerformance.EditedBy = User.Identity.Name;
            singlePerformance.Edited = DateTime.Now;
            singlePerformance.Status = viewModel.Status;
            singlePerformance.Event = Db.Events.Single(m => m.Id == viewModel.Event);
            singlePerformance.Artist = Db.Artists.Single(m => m.Id == viewModel.Artist);
            singlePerformance.Location = Db.Locations.Single(m => m.Id == viewModel.Location);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Performances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singlePerformance = Db.Performances.Find(id);

            singlePerformance.Artist.Performances.Remove(singlePerformance);
            singlePerformance.Status = false;
            singlePerformance.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Performances/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singlePerformance = Db.Performances.Find(id);

            singlePerformance.Status = !singlePerformance.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Performance> GetPerformances(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Performances : Db.Performances.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singlePerformance = Db.Performances.Find(id);
            return singlePerformance != null && !singlePerformance.Deleted;
        }
    }
}
