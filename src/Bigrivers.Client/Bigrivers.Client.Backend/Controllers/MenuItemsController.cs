using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Helpers;
using Bigrivers.Client.Backend.Models;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;

namespace Bigrivers.Client.Backend.Controllers
{
    public class MenuItemsController : BaseController
    {

        private static FileUploadValidator LinkFileValidator
        {
            get
            {
                return new FileUploadValidator
                {
                    Required = true,
                    MaxByteSize = 2000000,
                    MimeTypes = new string[] { },
                    ModelErrors = new FileUploadModelErrors
                    {
                        Required = "Er moet een bestand worden geupload",
                        ExceedsMaxByteSize = "Het bestand mag niet groter zijn dan 2 MB",
                    }
                };
            }
        }
        private static string FileLinkUploadLocation { get { return Helpers.FileUploadLocation.LinkUpload; } }

        // GET: MenuItem/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: MenuItem/
        public ActionResult Manage()
        {
            // Place all menu items to unsorted list
            var menuItems = GetMenuItems();

            // Set all parent items into the model first
            var model = menuItems
                .Where(m => m.Status && m.Parent == null)
                .OrderBy(m => m.Order)
                .ToList();

            // Put all parents into a selectlist now the children aren't added yet
            ViewBag.menuParents = model.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.DisplayName
            }).ToList();

            // After every parent p, insert all children into the model
            foreach (var p in model.ToList())
            {
                var list = menuItems
                    .Where(m => m.Parent == p.Id)
                    .OrderBy(m => m.Order)
                    .ToList();
                model.InsertRange(model.FindLastIndex(m => m.Id == p.Id) + 1, list);
            }
            // Finally, add all inactive parents to the end of the model
            model.AddRange(menuItems
                .Where(m => !m.Status));

            // Add an option to the selectlist for no parent at the top of the selectlist
            ViewBag.menuParents
                .Insert(0, new SelectListItem
                {
                    Value = "-1",
                    Text = "Geen"
                });

            ViewBag.Title = "MenuItems";
            return View(model);
        }

        // GET: MenuItem/Create
        public ActionResult New()
        {
            // Add an empty entry to lists to select a single object, e.g. /Artists/5, so there can exist a /Artists/
            var model = new MenuItemViewModel
            {
                LinkView = new LinkViewModel
                {
                    LinkType = "internal",
                    InternalType = "Events",
                    File = new FileUploadViewModel
                    {
                        NewUpload = true,
                        FileBase = Db.Files.Where(m => m.Container == FileLinkUploadLocation).ToList()
                    }
                },
                Status = true
            };

            ViewBag.Title = "Nieuw MenuItem";
            return View("Edit", model);
        }

        // POST: MenuItem/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(MenuItemViewModel model)
        {
            model.LinkView.File.FileBase = Db.Files.Where(m => m.Container == FileLinkUploadLocation).ToList();

            if (model.LinkView.LinkType == "file")
            {
                // Run over a validator to add custom model errors
                foreach (var error in LinkFileValidator.CheckFile(model.LinkView.File))
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw MenuItem";
                return View("Edit", model);
            }

            // Set the new item as the last item in the root order
            var item = Db.MenuItems
                .OrderByDescending(m => m.Order)
                .FirstOrDefault(m => m.Status);
            var order = item != null ? item.Order + 1 : 1;

            var link = LinkManageHelper.SetLink(model.LinkView);

            var singleMenuItem = new MenuItem
            {
                Target = Db.Links.SingleOrDefault(m => m.Id == link.Id),
                DisplayName = model.DisplayName,
                Order = order,
                EditedBy = User.Identity.Name,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                Status = model.Status
            };

            Db.MenuItems.Add(singleMenuItem);
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);

            // Create the new model with everything in it.
            var viewModel = new MenuItemViewModel
            {
                DisplayName = singleMenuItem.DisplayName,
                Status = singleMenuItem.Status,
                LinkView = LinkManageHelper.SetViewModel(singleMenuItem.Target, new LinkViewModel
                {
                    File = new FileUploadViewModel
                    {
                        NewUpload = true,
                        ExistingFile = singleMenuItem.Target.File,
                        FileBase = Db.Files.Where(m => m.Container == FileLinkUploadLocation).ToList()
                    }
                }),
            };

            ViewBag.Title = "Bewerk MenuItem";
            return View(viewModel);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MenuItemViewModel model)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);

            model.LinkView.File.ExistingFile = singleMenuItem.Target.File;
            model.LinkView.File.FileBase = Db.Files.Where(m => m.Container == FileLinkUploadLocation).ToList();

            if (model.LinkView.LinkType == "file")
            {
                // Run over a validator to add custom model errors
                foreach (var error in LinkFileValidator.CheckFile(model.LinkView.File))
                {
                    ModelState.AddModelError("", error);
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw MenuItem";
                return View("Edit", model);
            }

            var link = singleMenuItem.Target;
            if (model.LinkView.LinkType == "file" && model.LinkView.File.UploadFile != null || model.LinkView.File.Key != null && model.LinkView.File.Key != "false")
            {
                link = LinkManageHelper.SetLink(model.LinkView);
            }
            else if (model.LinkView.LinkType != "file")
            {
                link = LinkManageHelper.SetLink(model.LinkView);
            }

            singleMenuItem.DisplayName = model.DisplayName;
            singleMenuItem.EditedBy = User.Identity.Name;
            singleMenuItem.Edited = DateTime.Now;
            singleMenuItem.Status = model.Status;
            singleMenuItem.Target = Db.Links.Single(m => m.Id == link.Id);

            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: MenuItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);

            // Unlink all children and set inactive
            var children = Db.MenuItems.Where(m => m.Parent == id).ToList();
            if (children.Count > 0)
            {
                foreach (var c in children)
                {
                    c.Parent = null;
                    c.Order = null;
                    c.Status = false;
                }
            }

            singleMenuItem.Order = null;
            singleMenuItem.IsParent = false;
            singleMenuItem.Status = false;
            singleMenuItem.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: MenuItem/SwitchStatus/5
        // Switch boolean Status from menuitem to opposite value
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);

            singleMenuItem.Status = !singleMenuItem.Status;
            switch (singleMenuItem.Status)
            {
                case true:
                    // Set the menuitem at the end of the root order
                    singleMenuItem.Order = Db.MenuItems.OrderByDescending(m => m.Order).First(m => m.Status).Order + 1 ?? 1;
                    break;
                case false:
                    singleMenuItem.Order = null;
                    singleMenuItem.IsParent = false;

                    // Unlink and deactivate all children from the parent
                    var children = Db.MenuItems.Where(m => m.Parent == id).ToList();
                    if (children.Count > 0)
                    {
                        foreach (var c in children)
                        {
                            c.Parent = null;
                            c.Order = null;
                            c.Status = false;
                        }
                    }
                    break;
            }
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: MenuItems/ShiftOrder/5/up
        // Switch order of menuitems in direction param with menuitem above / below
        public ActionResult ShiftOrder(int? id, string param)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            if (param != "up" && param != "down") return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);

            MenuItem switchItem = null;
            switch (param)
            {
                case "up":
                    {
                        // Go to Manage if singleMenuItem already is the first item
                        if (Db.MenuItems
                            .OrderBy(m => m.Order)
                            .First() == singleMenuItem) return RedirectToAction("Manage");

                        switchItem = Db.MenuItems
                            .Where(m => m.Order < singleMenuItem.Order && m.Status && m.Parent == singleMenuItem.Parent)
                            .OrderByDescending(m => m.Order)
                            .FirstOrDefault();
                        break;
                    }
                case "down":
                    {
                        // Go to Manage if singleMenuItem already is the last item
                        // OrderDescending.First() is used because of issues from SQL limitations with the Last() method being used in this scenario
                        if (Db.MenuItems
                            .OrderByDescending(m => m.Order)
                            .First() == singleMenuItem) return RedirectToAction("Manage");

                        switchItem = Db.MenuItems
                            .Where(m => m.Order > singleMenuItem.Order && m.Status && m.Parent == singleMenuItem.Parent)
                            .OrderBy(m => m.Order)
                            .FirstOrDefault();
                        break;
                    }
            }
            if (switchItem == null || switchItem.Deleted) return RedirectToAction("Manage");

            // Save order of the item to switch with in temporary variable
            var t = switchItem.Order;
            switchItem.Order = singleMenuItem.Order;
            singleMenuItem.Order = t;

            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        public ActionResult AddToParent(int? id, int? param)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            if (param == null) return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);
            var oldParent = Db.MenuItems
                .FirstOrDefault(m => m.Id == singleMenuItem.Parent);

            // Remove the IsParent property of the previous parent if it doesn't have more children
            if (oldParent != null && Db.MenuItems
                .Where(m => m.Parent == oldParent.Id)
                .ToList()
                .Count <= 1)
            {
                oldParent.IsParent = false;
            }

            // Set the new parent value
            singleMenuItem.Parent = null;
            if (param.Value != -1)
            {
                singleMenuItem.Parent = param.Value;
                var parent = Db.MenuItems
                    .FirstOrDefault(m => m.Id == singleMenuItem.Parent);
                parent.IsParent = true;
            }

            // Get last menuitem in order with same parent as singleMenuItem
            var item = Db.MenuItems
                .Where(m => m.Parent == singleMenuItem.Parent)
                .OrderByDescending(m => m.Order)
                .FirstOrDefault(m => m.Status);

            // Set the item in the last position of the menu
            var order = item != null ? item.Order + 1 : 1;
            singleMenuItem.Order = order;

            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<MenuItem> GetMenuItems(bool includeDeleted = false)
        {
            return includeDeleted ? Db.MenuItems : Db.MenuItems.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleMenuItem = Db.MenuItems.Find(id);
            return singleMenuItem != null && !singleMenuItem.Deleted;
        }
    }
}