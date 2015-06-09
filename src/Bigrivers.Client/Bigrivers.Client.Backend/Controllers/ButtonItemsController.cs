using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Models;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Client.Backend.Helpers;

namespace Bigrivers.Client.Backend.Controllers
{
    public class ButtonItemsController : BaseController
    {
        private static FileUploadValidator FileValidator
        {
            get
            {
                return new FileUploadValidator
                {
                    Required = true,
                    MaxByteSize = 2000000,
                    MimeTypes = new[] { "image" },
                    ModelErrors = new FileUploadModelErrors
                    {
                        Required = "Er moet een afbeelding worden geupload",
                        ExceedsMaxByteSize = "De afbeelding mag niet groter zijn dan 2 MB",
                        ForbiddenMime = "Het bestand moet een afbeelding zijn"
                    }
                };
            }
        }

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

        private static string FileUploadLocation { get { return Helpers.FileUploadLocation.ButtonLogo; } }
        private static string FileLinkUploadLocation { get { return Helpers.FileUploadLocation.LinkUpload; } }

        // GET: ButtonItems/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: ButtonItems/
        public ActionResult Manage()
        {
            // All menu items to unsorted list
            var buttonItems = GetButtonItems();

            // Set all active items into new list first
            var model = buttonItems
                .Where(m => m.Status)
                .OrderBy(m => m.Order)
                .ToList();
            // Finally, add all inactive items to the end of the list
            model.AddRange(buttonItems
                .Where(m => !m.Status));
            ViewBag.Title = "ButtonItems";
            return View("Manage", model);
        }

        // GET: ButtonItems/Create
        public ActionResult New()
        {
            var model = new ButtonItemViewModel
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
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList()
                },
                Status = true
            };

            ViewBag.Title = "Nieuw ButtonItem";
            return View("Edit", model);
        }

        // POST: ButtonItems/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ButtonItemViewModel model)
        {
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList();
            model.LinkView.File.FileBase = Db.Files.Where(m => m.Container == FileLinkUploadLocation).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileValidator.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }
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
                ViewBag.Title = "Nieuw ButtonItem";
                return View("Edit", model);
            }

            File photoEntity = null;
            // Either upload file to AzureStorage or use file Key from explorer to get the file
            if (model.Image.NewUpload)
            {
                if (model.Image.UploadFile != null)
                {
                    photoEntity = FileUploadHelper.UploadFile(model.Image.UploadFile, FileUploadLocation);
                }
            }
            else
            {
                if (model.Image.Key != "false")
                {
                    photoEntity = Db.Files.Single(m => m.Key == model.Image.Key);
                }
            }

            // Set item's order as last item in list
            var order = Db.ButtonItems.Count(m => m.Status) > 0 ? Db.ButtonItems.OrderByDescending(m => m.Order).First().Order + 1 : 1;

            var link = LinkManageHelper.SetLink(model.LinkView);
            var singleButtonItem = new ButtonItem
            {
                Target = Db.Links.SingleOrDefault(m => m.Id == link.Id),
                DisplayName = model.DisplayName,
                Order = order,
                Type = ButtonType.Regular,
                EditedBy = User.Identity.Name,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                Status = model.Status,
                Logo = photoEntity != null ? Db.Files.Single(m => m.Key == photoEntity.Key) : null
            };

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
                DisplayName = singleButtonItem.DisplayName,
                Status = singleButtonItem.Status,
                LinkView = LinkManageHelper.SetViewModel(singleButtonItem.Target, new LinkViewModel
                {
                    File = new FileUploadViewModel
                    {
                        NewUpload = true,
                        ExistingFile = singleButtonItem.Target.File,
                        FileBase = Db.Files.Where(m => m.Container == FileLinkUploadLocation).ToList()
                    }
                }),
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    ExistingFile = singleButtonItem.Logo,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList()
                }
            };
            ViewBag.Title = "Bewerk ButtonItem";
            return View(model);
        }

        // POST: ButtonItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ButtonItemViewModel model)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleButtonItem = Db.ButtonItems.Find(id);

            model.Image.ExistingFile = singleButtonItem.Logo;
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation).ToList();
            model.LinkView.File.ExistingFile = singleButtonItem.Target.File;
            model.LinkView.File.FileBase = Db.Files.Where(m => m.Container == FileLinkUploadLocation).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileValidator.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }
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

            File photoEntity = null;
            // Either upload file to AzureStorage or use file Key from explorer to get the file
            if (model.Image.NewUpload)
            {
                if (model.Image.UploadFile != null)
                {
                    photoEntity = FileUploadHelper.UploadFile(model.Image.UploadFile, FileUploadLocation);
                }
            }
            else
            {
                if (model.Image.Key != "false")
                {
                    photoEntity = Db.Files.Single(m => m.Key == model.Image.Key);
                }
            }

            var link = singleButtonItem.Target;
            if (model.LinkView.LinkType == "file" && model.LinkView.File.UploadFile != null || model.LinkView.File.Key != null && model.LinkView.File.Key != "false")
            {
                link = LinkManageHelper.SetLink(model.LinkView);
            }
            else if (model.LinkView.LinkType != "file")
            {
                link = LinkManageHelper.SetLink(model.LinkView);
            }

            singleButtonItem.DisplayName = model.DisplayName;
            singleButtonItem.EditedBy = User.Identity.Name;
            singleButtonItem.Edited = DateTime.Now;
            singleButtonItem.Status = model.Status;
            singleButtonItem.Target = Db.Links.Single(m => m.Id == link.Id);
            if (photoEntity != null) singleButtonItem.Logo = Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key);
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
                    singleButtonItem.Order = Db.ButtonItems.OrderByDescending(m => m.Order).First().Order + 1;
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