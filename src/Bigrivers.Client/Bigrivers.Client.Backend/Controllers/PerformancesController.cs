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
    public class PerformancesController : Controller
    {
        // TODO: Create BaseController class for BigRiversDb
        private readonly BigriversDb _db = new BigriversDb();

        // GET: MenuItem/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: MenuItem/
        public ActionResult Manage()
        {
            // List all artists and return view
            ViewBag.listPerformances = _db.Performances.Where(m => !m.Deleted).ToList();

            ViewBag.Title = "Optredens";
            return View("Manage");
        }

        // GET: MenuItem/Create
        public ActionResult New()
        {
            var viewModel = new PerformanceViewModel
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(1),
                Status = true
            };

            ViewBag.Title = "Nieuw Optreden";
            return View("Edit", viewModel);
        }

        // POST: MenuItem/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(PerformanceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw Optreden";
                return View("Edit", viewModel);
            }

            //ViewBag.Title = "Resultaat - Example";
            var singlePerformance = new Performance
            {
                Description = viewModel.Description,
                Start = viewModel.Start,
                End = viewModel.End,
                Status = viewModel.Status
            };

            _db.Performances.Add(singlePerformance);
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");

            // Find single menuitem
            var singlePerformance = _db.Performances.Find(id);

            // Send to Manage view if menuitem is not found
            if (singlePerformance == null || singlePerformance.Deleted) return RedirectToAction("Manage");

            var model = new PerformanceViewModel
            {
                Description = singlePerformance.Description,
                Start = singlePerformance.Start.DateTime,
                End = singlePerformance.End.DateTime,
                Status = singlePerformance.Status
            };

            ViewBag.Title = "Bewerk Optreden";
            return View(model);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PerformanceViewModel viewModel)
        {
            var singlePerformance = _db.Performances.Find(id);

            singlePerformance.Description = viewModel.Description;
            singlePerformance.Start = viewModel.Start;
            singlePerformance.End = viewModel.End;
            singlePerformance.Status = viewModel.Status;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: MenuItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singlePerformance = _db.Performances.Find(id);

            // Send to Manage view if menuitem is not found
            if (singlePerformance == null || singlePerformance.Deleted) return RedirectToAction("Manage");

            singlePerformance.Deleted = true;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singlePerformance = _db.Performances.Find(id);

            // Send to Manage view if menuitem is not found
            if (singlePerformance == null || singlePerformance.Deleted) return RedirectToAction("Manage");

            singlePerformance.Status = !singlePerformance.Status;
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
