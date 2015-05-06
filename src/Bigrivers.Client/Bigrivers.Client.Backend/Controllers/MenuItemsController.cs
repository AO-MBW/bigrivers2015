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
using Microsoft.Ajax.Utilities;

namespace Bigrivers.Client.Backend.Controllers
{
    public class MenuItemsController : Controller
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
            ViewBag.listMenuItems = _db.MenuItems.Where(m => !m.Deleted).OrderBy(m => m.Order).ToList();

            ViewBag.Title = "MenuItems";
            return View("Manage");
        }

        // GET: MenuItem/Create
        public ActionResult New()
        {
            var viewModel = new MenuItemViewModel
            {
                Status = true
            };

            ViewBag.Title = "Nieuw MenuItem";
            return View("Edit", viewModel);
        }

        // POST: MenuItem/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(MenuItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw MenuItem";
                return View("Edit", viewModel);
            }

            //ViewBag.Title = "Resultaat - Example";
            var singleMenuItem = new MenuItem
            {
                URL = viewModel.URL,
                DisplayName = viewModel.DisplayName,
                Order = _db.MenuItems.OrderByDescending(m => m.Order).First().Order+1,
                Parent = 0,
                Status = viewModel.Status
            };

            _db.MenuItems.Add(singleMenuItem);
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");

            // Find single menuitem
            var singleMenuItem = _db.MenuItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            var model = new MenuItemViewModel
            {
                URL = singleMenuItem.URL,
                DisplayName = singleMenuItem.DisplayName,
                Parent = singleMenuItem.Parent,
                Status = singleMenuItem.Status
            };
            ViewBag.Title = "Bewerk MenuItem";
            return View(model);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MenuItemViewModel viewModel)
        {
            var singleMenuItem = _db.MenuItems.Find(id);

            singleMenuItem.URL = viewModel.URL;
            singleMenuItem.DisplayName = viewModel.DisplayName;
            singleMenuItem.Parent = viewModel.Parent;
            singleMenuItem.Status = viewModel.Status;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: MenuItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleMenuItem = _db.MenuItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            singleMenuItem.Order = null;
            singleMenuItem.Deleted = true;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/SwitchStatus/5
        // Switch boolean Status from menuitem to opposite value
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleMenuItem = _db.MenuItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            singleMenuItem.Status = !singleMenuItem.Status;
            switch (singleMenuItem.Status)
            {
                case true:
                    singleMenuItem.Order = _db.MenuItems.OrderByDescending(m => m.Order).First().Order + 1;
                    break;
                case false:
                    singleMenuItem.Order = null;
                    break;
            }
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: MenuItem/OrderUp/5
        // Switch menuitem order property with the menuitem above
        public ActionResult OrderUp(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleMenuItem = _db.MenuItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            // Go to Manage if singleMenuItem already is the first item
            if (_db.MenuItems.OrderBy(m => m.Order).First() == singleMenuItem) return RedirectToAction("Manage");

            var nextMenuItem = _db.MenuItems.Single(m => m.Order == (singleMenuItem.Order - 1));
            if (nextMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");
            nextMenuItem.Order++;
            singleMenuItem.Order--;
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: MenuItem/OrderDown/5
        // Switch menuitem order property with the menuitem below
        public ActionResult OrderDown(int? id)
        {
            if (id == null) return RedirectToAction("Manage");

            var singleMenuItem = _db.MenuItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            // Go to Manage if singleMenuItem already is the last item
            // OrderDescending.First() is used because of issues from SQL limitations with the Last() method being used without setting the result to a list first
            if (_db.MenuItems.OrderByDescending(m => m.Order).First() == singleMenuItem) return RedirectToAction("Manage");

            var previousMenuItem = _db.MenuItems.Single(m => m.Order == (singleMenuItem.Order + 1));
            if (previousMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");
            previousMenuItem.Order--;
            singleMenuItem.Order++;
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
