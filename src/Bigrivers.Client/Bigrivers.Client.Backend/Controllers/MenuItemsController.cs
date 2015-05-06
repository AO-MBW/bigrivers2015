using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;
using Microsoft.Ajax.Utilities;

namespace Bigrivers.Client.Backend.Controllers
{
    public class MenuItemsController : BaseController
    {
        // GET: MenuItem/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: MenuItem/
        public ActionResult Manage()
        {
            // All menu items
            ViewBag.listMenuItems = Db.MenuItems.Where(m => !m.Deleted).OrderBy(m => m.Order).ToList();

            // All menu items as selectlist
            ViewBag.menuparents = Db.MenuItems
                .Where(m => m.Status && m.Parent == null)
                .Select(s => new SelectListItem{
                    Value = s.Id.ToString(),
                    Text = s.DisplayName
                })
                .ToList();

            ViewBag.menuparents.Insert(0, new SelectListItem
            {
                Value = "-1",
                Text = "Geen"
            });
            ViewBag.Title = "MenuItems";
            return View();
        }

        public ActionResult Search(string id)
        {
            var search = id;
            ViewBag.listMenuItems = Db.MenuItems.Where(m => !m.Deleted && m.DisplayName.Contains(search)).OrderBy(m => m.Order).ToList();

            ViewBag.Title = "Zoek MenuItems";
            return View("Manage");
        }

        // GET: MenuItem/Create
        public ActionResult New()
        {
            var viewModel = new ButtonItemViewModel
            {
                Status = true
            };

            ViewBag.Title = "Nieuw MenuItem";
            return View("Edit", viewModel);
        }

        // POST: MenuItem/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ButtonItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw MenuItem";
                return View("Edit", viewModel);
            }

            var item = Db.MenuItems.OrderByDescending(m => m.Order).FirstOrDefault(m => !m.Status);
            var order = item != null ? item.Order + 1 : 1;

            var singleMenuItem = new MenuItem
            {
                URL = viewModel.URL,
                DisplayName = viewModel.DisplayName,
                Order = order,
                Status = viewModel.Status
            };

            Db.MenuItems.Add(singleMenuItem);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");
            var singleMenuItem = Db.MenuItems.Find(id);
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            var model = new ButtonItemViewModel
            {
                URL = singleMenuItem.URL,
                DisplayName = singleMenuItem.DisplayName,
                Status = singleMenuItem.Status
            };
            ViewBag.Title = "Bewerk MenuItem";
            return View(model);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ButtonItemViewModel viewModel)
        {
            var singleMenuItem = Db.MenuItems.Find(id);

            singleMenuItem.URL = viewModel.URL;
            singleMenuItem.DisplayName = viewModel.DisplayName;
            singleMenuItem.Status = viewModel.Status;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: MenuItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            singleMenuItem.Order = null;
            singleMenuItem.Status = false;
            singleMenuItem.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/SwitchStatus/5
        // Switch boolean Status from menuitem to opposite value
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            singleMenuItem.Status = !singleMenuItem.Status;
            switch (singleMenuItem.Status)
            {
                case true:
                    singleMenuItem.Order = Db.MenuItems.OrderByDescending(m => m.Order).First(m => m.Status).Order + 1 ?? 1;
                    break;
                case false:
                    singleMenuItem.Order = null;
                    break;
            }
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: MenuItems/ShiftOrder/5/up
        // Switch order of menuitems in direction param with menuitem above / below
        public ActionResult ShiftOrder(int? id, string param)
        {
            if (id == null || param == null) return RedirectToAction("Manage");
            if (param != "up" && param != "down") return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            switch (param)
            {
                case "up":
                {
                    // Go to Manage if singleMenuItem already is the first item
                    if (Db.MenuItems.OrderBy(m => m.Order).First() == singleMenuItem) return RedirectToAction("Manage");

                    var nextMenuItem = Db.MenuItems.Where(m => m.Order < singleMenuItem.Order && m.Status && m.Parent == singleMenuItem.Parent).OrderByDescending(m => m.Order).FirstOrDefault();
                    if (nextMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");
                    var neworder = nextMenuItem.Order;
                    nextMenuItem.Order = singleMenuItem.Order;
                    singleMenuItem.Order = neworder;
                    break;
                }    
                case "down":
                {
                    // Go to Manage if singleMenuItem already is the last item
                    // OrderDescending.First() is used because of issues from SQL limitations with the Last() method being used in this scenario
                    if (Db.MenuItems.OrderByDescending(m => m.Order).First() == singleMenuItem) return RedirectToAction("Manage");

                    var previousMenuItem = Db.MenuItems.Where(m => m.Order > singleMenuItem.Order && m.Status && m.Parent == singleMenuItem.Parent).OrderBy(m => m.Order).FirstOrDefault();
                    if (previousMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");
                    var neworder = previousMenuItem.Order;
                    previousMenuItem.Order = singleMenuItem.Order;
                    singleMenuItem.Order = neworder;
                    break;
                }
                    
            }

            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        public ActionResult AddToParent(int? id, int? param)
        {
            if (id == null || param == null) return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            singleMenuItem.Parent = null;
            if (param.Value != -1) singleMenuItem.Parent = param.Value;

            // Get last menuitem in order with same parent as singleMenuItem
            var item = Db.MenuItems
                .Where(m => m.Parent == singleMenuItem.Parent)
                .OrderByDescending(m => m.Order)
                .FirstOrDefault(m => m.Status);

            var order = item != null ? item.Order + 1 : 1;
            singleMenuItem.Order = order;

            Db.SaveChanges();
            return RedirectToAction("Manage");
        }
    }
}