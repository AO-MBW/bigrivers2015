using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Client.Helpers;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;

namespace Bigrivers.Client.Backend.Controllers
{
    public class ButtonItemsController : BaseController
    {

        // GET: ButtonItems/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: ButtonItems/
        public ActionResult Manage()
        {
            // All menu items to unsorted list
            var buttonItems = GetButtonItems()
                .ToList();

            // Set all active items into new list first
            var listButtonItems = buttonItems
                .Where(m => m.Status)
                .OrderBy(m => m.Order)
                .ToList();
            // Finally, add all inactive items to the end of the list
            listButtonItems
                .AddRange(buttonItems.Where(m => !m.Status)
                .ToList());

            ViewBag.listButtonItems = listButtonItems;
            ViewBag.Title = "ButtonItems";
            return View("Manage");
        }

        public ActionResult Search(string id)
        {
            var search = id;
            ViewBag.listButtonItems = GetButtonItems()
                .Where(m => m.DisplayName.Contains(search))
                .OrderBy(m => m.Order)
                .ToList();

            ViewBag.Title = "Zoek ButtonItems";
            return View("Manage");
        }

        // GET: ButtonItems/Create
        public ActionResult New()
        {
            var viewModel = new ButtonItemViewModel
            {
                Status = true
            };

            ViewBag.Title = "Nieuw ButtonItem";
            return View("Edit", viewModel);
        }

        // POST: ButtonItems/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ButtonItemViewModel viewModel, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw ButtonItem";
                return View("Edit", viewModel);
            }

            File photoEntity = null;
            if (file != null)
            {
                
                if (ImageHelper.IsSize(file, 200000) && ImageHelper.IsMimes(file, new[] {"image"}))
                {
                    photoEntity = ImageHelper.UploadFile(file, "buttonitem");
                }
                else
                {
                    return RedirectToAction("Manage");
                }
            }
            

            // Set item's order as last item in list
            var order = Db.ButtonItems.Count(m => m.Status) > 0 ? Db.ButtonItems.OrderByDescending(m => m.Order).First().Order + 1 : 1;

            var singleButtonItem = new ButtonItem
            {
                URL = viewModel.URL,
                DisplayName = viewModel.DisplayName,
                Order = order,
                Status = viewModel.Status,
                Logo = photoEntity
            };

            // Check if file is not yet uploaded
            if (photoEntity != null && !Db.Files.Any(m => m.Md5 == photoEntity.Md5 && m.Container == photoEntity.Container)) Db.Files.Add(photoEntity);
            Db.ButtonItems.Add(singleButtonItem);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: ButtonItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleButtonItem = Db.ButtonItems.Find(id);

            var model = new ButtonItemViewModel
            {
                URL = singleButtonItem.URL,
                DisplayName = singleButtonItem.DisplayName,
                Status = singleButtonItem.Status
            };
            ViewBag.Title = "Bewerk ButtonItem";
            return View(model);
        }

        // POST: ButtonItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ButtonItemViewModel viewModel, HttpPostedFileBase file)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleButtonItem = Db.ButtonItems.Find(id);

            File photoEntity = null;
            if (file != null)
            {
                if (ImageHelper.IsSize(file, 200000) && ImageHelper.IsMimes(file, new[] {"image"}))
                {
                    photoEntity = ImageHelper.UploadFile(file, "buttonitem");
                }
            }

            singleButtonItem.URL = viewModel.URL;
            singleButtonItem.DisplayName = viewModel.DisplayName;
            singleButtonItem.Status = viewModel.Status;
            if (photoEntity != null && !Db.Files.Any(m => m.Md5 == photoEntity.Md5)) singleButtonItem.Logo = photoEntity;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: ButtonItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleButtonItem = Db.ButtonItems.Find(id);

            singleButtonItem.Order = null;
            singleButtonItem.Status = false;
            singleButtonItem.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: ButtonItems/SwitchStatus/5
        // Switch boolean Status from menuitem to opposite value
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");

            var singleButtonItem = Db.ButtonItems.Find(id);

            singleButtonItem.Status = !singleButtonItem.Status;
            switch (singleButtonItem.Status)
            {
                case true:
                    singleButtonItem.Order = Db.MenuItems.OrderByDescending(m => m.Order).First().Order + 1;
                    break;
                case false:
                    singleButtonItem.Order = null;
                    break;
            }
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: ButtonItems/ShiftOrder/5/up
        // Switch order of buttonitems in direction param with buttonitem above / below
        public ActionResult ShiftOrder(int? id, string param)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            if (param != "up" && param != "down") return RedirectToAction("Manage");
            var singleButtonItem = Db.ButtonItems.Find(id);

            ButtonItem switchItem = null;
            switch (param)
            {
                case "up":
                {
                    // Go to Manage if singleButtonItem already is the first item
                    if (Db.ButtonItems
                        .OrderBy(m => m.Order)
                        .First() == singleButtonItem) return RedirectToAction("Manage");

                    switchItem = Db.ButtonItems
                        .Where(m => m.Order < singleButtonItem.Order && m.Status)
                        .OrderByDescending(m => m.Order)
                        .FirstOrDefault();
                    break;
                }
                case "down":
                {
                    // Go to Manage if singleButtonItem already is the last item
                    // OrderDescending.First() is used because of issues from SQL limitations with the Last() method being used in this scenario
                    if (Db.ButtonItems
                        .OrderByDescending(m => m.Order)
                        .First() == singleButtonItem) return RedirectToAction("Manage");

                    switchItem = Db.ButtonItems
                        .Where(m => m.Order > singleButtonItem.Order && m.Status)
                        .OrderBy(m => m.Order)
                        .FirstOrDefault();
                    break;
                }
            }
            if (switchItem == null || singleButtonItem.Deleted) return RedirectToAction("Manage");

            // Save order of the item to switch with in temporary variable
            var t = switchItem.Order;
            switchItem.Order = singleButtonItem.Order;
            singleButtonItem.Order = t;

            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<ButtonItem> GetButtonItems(bool includeDeleted = false)
        {
            return includeDeleted ? Db.ButtonItems : Db.ButtonItems.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleButtonItem = Db.ButtonItems.Find(id);
            return singleButtonItem != null && !singleButtonItem.Deleted;
        }
    }
}