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
    public class MenuItemController : Controller
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
            ViewBag.listMenuItems = _db.MenuItems.ToList();

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
                Order = 0,
                Parent = 0,
                Status = viewModel.Status
            };

            _db.MenuItems.Add(singleMenuItem);
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int id)
        {
            // Find single menuitem
            var singleMenuItem = _db.MenuItems.Find(id);

            // Send to Manage view if menuitem is not found
            if (singleMenuItem == null) return RedirectToAction("Manage");

            var model = new MenuItemViewModel
            {
                URL = singleMenuItem.URL,
                DisplayName = singleMenuItem.DisplayName,
                Order = singleMenuItem.Order,
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
            singleMenuItem.Order = viewModel.Order;
            singleMenuItem.Parent = viewModel.Parent;
            singleMenuItem.Status = viewModel.Status;
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: MenuItem/Delete/5
        public ActionResult Delete(int id)
        {
            // TODO: Add delete logic here
            var singleMenuItem = _db.MenuItems.Find(id);

            _db.MenuItems.Remove(singleMenuItem);
            _db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/SwitchStatus/5
        public ActionResult SwitchStatus(int id)
        {
            var singleMenuItem = _db.MenuItems.Find(id);

            singleMenuItem.Status = !singleMenuItem.Status;
            _db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
