using System.Linq;
using System.Web.Mvc;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;

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
            // All menu items to unsorted list
            var menuItems = GetMenuItems().ToList();

            // Set all active parents into new list first
            var listMenuItems = menuItems.Where(m => m.Status && m.Parent == null).OrderBy(m => m.Order).ToList();

            // Now all parents are selected, put them into a selectlist
            ViewBag.menuParents = listMenuItems.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.DisplayName
            }).ToList();

            // After every parent p, insert all children into the list
            foreach (var p in listMenuItems.ToList())
            {
                var list = menuItems
                    .Where(m => m.Parent == p.Id)
                    .OrderBy(m => m.Order)
                    .ToList();
                listMenuItems
                    .InsertRange(listMenuItems.FindLastIndex(m => m.Id == p.Id)+1, list);
            }
            // Finally, add all inactive parents to the end of the list
            listMenuItems.AddRange(menuItems.Where(m => !m.Status).ToList());

            // Add an option to the selectlist for no parent at top of list
            ViewBag.menuParents.Insert(0, new SelectListItem
            {
                Value = "-1",
                Text = "Geen"
            });
            ViewBag.listMenuItems = listMenuItems;
            ViewBag.Title = "MenuItems";
            return View();
        }

        public ActionResult Search(string id)
        {
            var search = id;
            var menuItems = GetMenuItems().Where(m => m.DisplayName.Contains(search)).ToList();

            var listMenuItems = menuItems.Where(m => m.Status && m.Parent == null).OrderBy(m => m.Order).ToList();

            ViewBag.Title = "Zoek MenuItems";
            return View("Manage");
        }

        // GET: MenuItem/Create
        public ActionResult New()
        {
            // Add an empty entry to lists to select a single object, e.g. /Artists/5, so there can exist a /Artists/
            var model = new MenuItemViewModel
            {
                LinkType = "internal",
                InternalType = "Events",
                Status = true,
                Events = Db.Events
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Title
                    }).ToList(),
                Artists = Db.Artists
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList(),
                Performances = Db.Performances
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Artist.Name
                    }).ToList(),
                Sponsors = Db.Sponsors
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList(),
                NewsItems = Db.NewsItems
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Title
                    }).ToList()
            };

            model.Events.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            model.Artists.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            model.Sponsors.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            model.NewsItems.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            model.Performances.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });

            ViewBag.Title = "Nieuw MenuItem";
            return View("Edit", model);
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

            string url;
            // Set the url variable to a working url based on the link type
            switch (viewModel.LinkType)
            {
                case "internal":
                    var internalId = "";

                    switch (viewModel.InternalType)
                    {
                        case "Events":
                            internalId = viewModel.InternalEventId;
                            break;
                        case "Artists":
                            internalId = viewModel.InternalArtistId;
                            break;
                        case "Performances":
                            internalId = viewModel.InternalPerformanceId;
                            break;
                        case "News":
                            internalId = viewModel.InternalNewsId;
                            break;
                        case "Sponsors":
                            internalId = viewModel.InternalSponsorId;
                            break;
                    }

                    url = "/Home/" + viewModel.InternalType + "/" + internalId;
                    break;
                case "external":
                    url = viewModel.ExternalUrl;
                    if (!url.StartsWith("http")) url = "http://" + url;
                    break;
                case "file":
                    url = viewModel.ExternalUrl;
                    break;
                default:
                    url = viewModel.ExternalUrl;
                    break;
            }

            var item = Db.MenuItems.OrderByDescending(m => m.Order).FirstOrDefault(m => m.Status);
            var order = item != null ? item.Order + 1 : 1;

            var singleMenuItem = new MenuItem
            {
                URL = url,
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

            // Find type of link (internal, external, file) through URL
            // TODO: Set up RegEx-based system to find single object links (e.g. /Artists/5) and properly load the value to the page
            string linkType;
            var internalType = "Events";
            var internalId = "";
            var externalUrl = "";
            if (singleMenuItem.URL.Contains("http"))
            {
                linkType = "external";
                externalUrl = singleMenuItem.URL;
            }
            else if (singleMenuItem.URL.Contains("/Images/"))
            {
                linkType = "file";
            }
            else
            {
                linkType = "internal";
                var arr = singleMenuItem.URL.Split(new []{'/'});
                internalType = arr[2];
                internalId = arr[3];
            }

            // Create the new model with everything in it.
            var model = new MenuItemViewModel
            {
                ExternalUrl = externalUrl,
                DisplayName = singleMenuItem.DisplayName,
                Status = singleMenuItem.Status,
                LinkType = linkType,
                InternalType = internalType,
                Events = Db.Events
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Title
                    }).ToList(),
                Artists = Db.Artists
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList(),
                Performances = Db.Performances
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Artist.Name
                    }).ToList(),
                Sponsors = Db.Sponsors
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList(),
                NewsItems = Db.NewsItems
                    .Where(m => !m.Deleted)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Title
                    }).ToList()
            };

            switch (internalType)
            {
                case "Events":
                    model.InternalEventId = internalId;
                    break;
                case "Artists":
                    model.InternalArtistId = internalId;
                    break;
                case "Performances":
                    model.InternalPerformanceId = internalId;
                    break;
                case "News":
                    model.InternalNewsId = internalId;
                    break;
                case "Sponsors":
                    model.InternalSponsorId = internalId;
                    break;
            }

            // Add an empty entry to lists to select a single object, e.g. /Artists/5, so there can exist a /Artists/
            model.Events.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            model.Artists.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            model.Sponsors.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            model.NewsItems.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            model.Performances.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = ""
                });
            ViewBag.Title = "Bewerk MenuItem";
            return View(model);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MenuItemViewModel viewModel)
        {
            var singleMenuItem = Db.MenuItems.Find(id);
            string url;

            // Set the url variable to a working url based on the link type
            switch (viewModel.LinkType)
            {
                case "internal":
                    var internalId = "";

                    switch (viewModel.InternalType)
                    {
                        case "Events":
                            internalId = viewModel.InternalEventId;
                            break;
                        case "Artists":
                            internalId = viewModel.InternalArtistId;
                            break;
                        case "Performances":
                            internalId = viewModel.InternalPerformanceId;
                            break;
                        case "News":
                            internalId = viewModel.InternalNewsId;
                            break;
                        case "Sponsors":
                            internalId = viewModel.InternalSponsorId;
                            break;
                    }

                    url = "/Home/" + viewModel.InternalType + "/" + internalId;
                    break;
                case "external":
                    url = viewModel.ExternalUrl;
                    break;
                case "file":
                    url = viewModel.ExternalUrl;
                    break;
                default:
                    url = viewModel.ExternalUrl;
                    break;
            }

            singleMenuItem.URL = url;
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
                    // Set the menuitem at the end of the root order
                    singleMenuItem.Order = Db.MenuItems.OrderByDescending(m => m.Order).First(m => m.Status).Order + 1 ?? 1;
                    break;
                case false:
                    singleMenuItem.Order = null;

                    // Unlick all children and set inactive
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
            if (id == null || param == null) return RedirectToAction("Manage");
            if (param != "up" && param != "down") return RedirectToAction("Manage");
            var singleMenuItem = Db.MenuItems.Find(id);
            if (singleMenuItem == null || singleMenuItem.Deleted) return RedirectToAction("Manage");

            // TODO: Refactor to minimize code
            switch (param)
            {
                case "up":
                {
                    // Go to Manage if singleMenuItem already is the first item
                    if (Db.MenuItems
                        .OrderBy(m => m.Order)
                        .First() == singleMenuItem) return RedirectToAction("Manage");

                    var nextMenuItem = Db.MenuItems
                        .Where(m => m.Order < singleMenuItem.Order && m.Status && m.Parent == singleMenuItem.Parent)
                        .OrderByDescending(m => m.Order)
                        .FirstOrDefault();
                    if (nextMenuItem == null || nextMenuItem.Deleted) return RedirectToAction("Manage");
                    var neworder = nextMenuItem.Order;
                    nextMenuItem.Order = singleMenuItem.Order;
                    singleMenuItem.Order = neworder;
                    break;
                }
                case "down":
                {
                    // Go to Manage if singleMenuItem already is the last item
                    // OrderDescending.First() is used because of issues from SQL limitations with the Last() method being used in this scenario
                    if (Db.MenuItems
                        .OrderByDescending(m => m.Order)
                        .First() == singleMenuItem) return RedirectToAction("Manage");

                    var previousMenuItem = Db.MenuItems
                        .Where(m => m.Order > singleMenuItem.Order && m.Status && m.Parent == singleMenuItem.Parent)
                        .OrderBy(m => m.Order)
                        .FirstOrDefault();
                    if (previousMenuItem == null || previousMenuItem.Deleted) return RedirectToAction("Manage");
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

            var oldParent = Db.MenuItems.FirstOrDefault(m => m.Id == singleMenuItem.Parent);
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

                var parent = Db.MenuItems.FirstOrDefault(m => m.Id == singleMenuItem.Parent);
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
    }
}