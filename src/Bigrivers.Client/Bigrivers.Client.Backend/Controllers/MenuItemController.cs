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
        private readonly BigriversDb db = new BigriversDb();

        // GET: MenuItem/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: MenuItem/
        public ActionResult Manage()
        {
            // List all artists and return view
            ViewBag.listMenuItems =
                (from m in db.MenuItems
                 select m).ToList();

            return View("Manage");
        }

        // GET: MenuItem/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: MenuItem/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(MenuItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            //ViewBag.Title = "Resultaat - Example";
            var m = new MenuItem
            {
                URL = viewModel.URL,
                DisplayName = viewModel.DisplayName,
                Order = viewModel.Order,
                Parent = viewModel.Parent,
                Status = true
            };

            db.MenuItems.Add(m);
            db.SaveChanges();

            ViewBag.ObjectAdded = viewModel;

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int id)
        {
            // Find single menuitem
            var menuItem =
                (from m in db.MenuItems
                 where m.Id == id
                 select m).First();

            // Send to Manage view if menuitem is not found
            if (menuItem == null) return RedirectToAction("Manage");

            var model = new MenuItemViewModel
            {
                URL = menuItem.URL,
                DisplayName = menuItem.DisplayName,
                Order = menuItem.Order,
                Parent = menuItem.Parent,
                Status = true
            };

            return View(model);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MenuItemViewModel viewModel)
        {
            var menuItem =
                (from m in db.MenuItems
                 where m.Id == id
                 select m).First();

            menuItem.URL = viewModel.URL;
            menuItem.DisplayName = viewModel.DisplayName;
            menuItem.Order = viewModel.Order;
            menuItem.Parent = viewModel.Parent;
            db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: MenuItem/Delete/5
        public ActionResult Delete(int id)
        {
            // TODO: Add delete logic here
            var menuItem =
                (from m in db.MenuItems
                 where m.Id == id
                 select m).First();

            db.MenuItems.Remove(menuItem);
            db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/SwitchStatus/5
        public ActionResult SwitchStatus(int id)
        {
            var menuItem =
            (from m in db.MenuItems
             where m.Id == id
             select m).First();

            menuItem.Status = !menuItem.Status;
            db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}
