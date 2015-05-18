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
            var performances = GetPerformances().ToList();
            var listPerformances = performances.Where(m => m.Status)
                .ToList();

            listPerformances.AddRange(performances.Where(m => !m.Status).ToList());
            ViewBag.listPerformances = listPerformances;

            ViewBag.Title = "Optredens";
            return View("Manage");
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
                Artist = null,
                Events = Db.Events
                    .Where(m => !m.Deleted)
                    .ToList()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Title
                    })
                    .ToList(),
                Artists = Db.Artists
                    .Where(m => !m.Deleted)
                    .ToList()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
                    .ToList()
            };

            ViewBag.Title = "Nieuw Optreden";
            return View("Edit", viewModel);
        }

        // POST: Performances/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(PerformanceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw Optreden";
                return View("Edit", viewModel);
            }

            var singlePerformance = new Performance
            {
                Description = viewModel.Description,
                Start = viewModel.Start,
                End = viewModel.End,
                Status = viewModel.Status,
                Event = Db.Events.Single(m => m.Id == viewModel.Event),
                Artist = Db.Artists.Single(m => m.Id == viewModel.Artist),
            };

            Db.Performances.Add(singlePerformance);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Performances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");
            var singlePerformance = Db.Performances.Find(id);
            if (singlePerformance == null || singlePerformance.Deleted) return RedirectToAction("Manage");

            var model = new PerformanceViewModel
            {
                Description = singlePerformance.Description,
                Start = singlePerformance.Start.DateTime,
                End = singlePerformance.End.DateTime,
                Status = singlePerformance.Status,
                Event = singlePerformance.Event.Id,
                Artist = singlePerformance.Artist.Id,
                Events = Db.Events
                    .Where(m => !m.Deleted)
                    .ToList()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Title
                    })
                    .ToList(),
                Artists = Db.Artists
                    .Where(m => !m.Deleted)
                    .ToList()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
                    .ToList()
            };

            // Set all active parents into new list first

            ViewBag.Title = "Bewerk Optreden";
            return View(model);
        }

        // POST: Performances/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PerformanceViewModel viewModel)
        {
            var singlePerformance = Db.Performances.Find(id);

            singlePerformance.Description = viewModel.Description;
            singlePerformance.Start = viewModel.Start;
            singlePerformance.End = viewModel.End;
            singlePerformance.Status = viewModel.Status;
            singlePerformance.Event = Db.Events.Single(m => m.Id == viewModel.Event);
            singlePerformance.Artist = Db.Artists.Single(m => m.Id == viewModel.Artist);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Performances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singlePerformance = Db.Performances.Find(id);
            if (singlePerformance == null || singlePerformance.Deleted) return RedirectToAction("Manage");

            singlePerformance.Artist.Performances.Remove(singlePerformance);
            singlePerformance.Status = false;
            singlePerformance.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Performances/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singlePerformance = Db.Performances.Find(id);
            if (singlePerformance == null || singlePerformance.Deleted) return RedirectToAction("Manage");

            singlePerformance.Status = !singlePerformance.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Performance> GetPerformances(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Performances : Db.Performances.Where(a => !a.Deleted);
        }
    }
}
