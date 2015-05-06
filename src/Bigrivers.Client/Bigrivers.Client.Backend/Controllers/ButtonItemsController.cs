using System;
using System.Collections;
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
    public class ButtonItemsController : Controller
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
            ViewBag.listButtonItems = _db.ButtonItems.Where(m => !m.Deleted).OrderBy(m => m.Order).ToList();

            ViewBag.Title = "ButtonItems";
            return View("Manage");
        }

        // GET: MenuItem/Create
        public ActionResult New()
        {
            var viewModel = new ButtonItemViewModel
            {
                Status = true
            };

            ViewBag.Title = "Nieuw ButtonItem";
            return View("Edit", viewModel);
        }

        // POST: MenuItem/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ButtonItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw ButtonItem";
                return View("Edit", viewModel);
            }

            //ViewBag.Title = "Resultaat - Example";
            var singleButtonItem = new ButtonItem
            {
                URL = viewModel.URL,
                DisplayName = viewModel.DisplayName,
                Order = _db.ButtonItems.OrderByDescending(m => m.Order).First().Order + 1,
                Status = viewModel.Status
            };

            _db.ButtonItems.Add(singleButtonItem);
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");

            // Find single menuitem
            var singleButtonItem = _db.ButtonItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleButtonItem == null || singleButtonItem.Deleted) return RedirectToAction("Manage");

            var model = new ButtonItemViewModel
            {
                URL = singleButtonItem.URL,
                DisplayName = singleButtonItem.DisplayName,
                Status = singleButtonItem.Status
            };
            ViewBag.Title = "Bewerk ButtonItem";
            return View(model);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ButtonItemViewModel viewModel)
        {
            var singleButtonItem = _db.ButtonItems.Find(id);

            singleButtonItem.URL = viewModel.URL;
            singleButtonItem.DisplayName = viewModel.DisplayName;
            singleButtonItem.Status = viewModel.Status;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: MenuItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleButtonItem = _db.ButtonItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleButtonItem == null || singleButtonItem.Deleted) return RedirectToAction("Manage");

            singleButtonItem.Order = null;
            singleButtonItem.Deleted = true;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/SwitchStatus/5
        // Switch boolean Status from menuitem to opposite value
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleButtonItem = _db.ButtonItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleButtonItem == null || singleButtonItem.Deleted) return RedirectToAction("Manage");

            singleButtonItem.Status = !singleButtonItem.Status;
            switch (singleButtonItem.Status)
            {
                case true:
                    singleButtonItem.Order = _db.MenuItems.OrderByDescending(m => m.Order).First().Order + 1;
                    break;
                case false:
                    singleButtonItem.Order = null;
                    break;
            }
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }

        public ActionResult ShiftOrder(int? id, string param)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleButtonItem = _db.ButtonItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleButtonItem == null || singleButtonItem.Deleted) return RedirectToAction("Manage");

            switch (param)
            {
                case "up":
                    // Go to Manage if singleMenuItem already is the first item
                    if (_db.ButtonItems.OrderBy(m => m.Order).First() == singleButtonItem) return RedirectToAction("Manage");

                    var nextMenuItem = _db.ButtonItems.Single(m => m.Order == (singleButtonItem.Order - 1));
                    if (nextMenuItem == null || singleButtonItem.Deleted) return RedirectToAction("Manage");
                    nextMenuItem.Order++;
                    singleButtonItem.Order--;
                    break;
                case "down":
                    // Go to Manage if singleMenuItem already is the last item
                    // OrderDescending.First() is used because of issues from SQL limitations with the Last() method being used without setting the result to a list first
                    if (_db.ButtonItems.OrderByDescending(m => m.Order).First() == singleButtonItem) return RedirectToAction("Manage");

                    var previousMenuItem = _db.ButtonItems.Single(m => m.Order == (singleButtonItem.Order + 1));
                    if (previousMenuItem == null || singleButtonItem.Deleted) return RedirectToAction("Manage");
                    previousMenuItem.Order--;
                    singleButtonItem.Order++;
                    break;
            }

            _db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
