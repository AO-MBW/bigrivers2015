﻿using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Models;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Client.Backend.Helpers;

namespace Bigrivers.Client.Backend.Controllers
{
    public class WidgetController : BaseController
    {
        // GET: Widget/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Widget/
        public ActionResult Manage()
        {
            // All widget items to unsorted list
            var widgetItems = GetWidgetItems();

            // Set all active items into new list first
            var model = widgetItems
                .Where(m => m.Status)
                .OrderBy(m => m.Order)
                .ToList();

            // Finally, add all inactive items to the end of the list
            model.AddRange(widgetItems
                .Where(m => !m.Status));

            ViewBag.Title = "Beheer widget";
            return View(model);
        }

        // GET: Widget/New
        public ActionResult New()
        {
            var model = new WidgetItemViewModel
            {
                LinkView = new LinkViewModel
                {
                    LinkType = "internal",
                    InternalType = "Events",
                    File = new FileUploadViewModel
                    {
                        NewUpload = true,
                        FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.LinkUpload).ToList()
                    }
                },
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.NewsWidget).ToList()
                },
                Status = true
            };

            ViewBag.Title = "Voeg toe aan widget";
            return View("Edit", model);
        }

        // POST: ButtonItems/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(WidgetItemViewModel model)
        {
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.NewsWidget).ToList();
            model.LinkView.File.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.LinkUpload).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileUploadValidator.ButtonItem.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }
            if (model.LinkView.LinkType == "file")
            {
                // Run over a validator to add custom model errors
                foreach (var error in FileUploadValidator.LinkFile.CheckFile(model.LinkView.File))
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Voeg toe aan widget";
                return View("Edit", model);
            }

            File photoEntity = null;
            // Either upload file to AzureStorage or use file Key from explorer to get the file
            if (model.Image.NewUpload)
            {
                if (model.Image.UploadFile != null)
                {
                    photoEntity = FileUploadHelper.UploadFile(model.Image.UploadFile, FileUploadLocation.NewsWidget);
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
            var singleWidgetItem = new WidgetItem
            {
                Target = Db.Links.SingleOrDefault(m => m.Id == link.Id),
                DisplayName = model.DisplayName,
                Order = order,
                EditedBy = User.Identity.Name,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                Status = model.Status,
                Image = photoEntity != null ? Db.Files.Single(m => m.Key == photoEntity.Key) : null
            };

            Db.WidgetItems.Add(singleWidgetItem);
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: ButtonItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleWidgetItem = Db.WidgetItems.Find(id);

            var model = new WidgetItemViewModel
            {
                DisplayName = singleWidgetItem.DisplayName,
                Status = singleWidgetItem.Status,
                LinkView = LinkManageHelper.SetViewModel(singleWidgetItem.Target, new LinkViewModel
                {
                    File = new FileUploadViewModel
                    {
                        NewUpload = true,
                        ExistingFile = singleWidgetItem.Target.File,
                        FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.LinkUpload).ToList()
                    }
                }),
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    ExistingFile = singleWidgetItem.Image,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.NewsWidget).ToList()
                }
            };
            ViewBag.Title = "Bewerk widget";
            return View(model);
        }

        // POST: ButtonItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, WidgetItemViewModel model)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleWidgetItem = Db.WidgetItems.Find(id);

            model.Image.ExistingFile = singleWidgetItem.Image;
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.NewsWidget).ToList();
            model.LinkView.File.ExistingFile = singleWidgetItem.Target.File;
            model.LinkView.File.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.LinkUpload).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileUploadValidator.ButtonItem.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }
            if (model.LinkView.LinkType == "file")
            {
                // Run over a validator to add custom model errors
                foreach (var error in FileUploadValidator.LinkFile.CheckFile(model.LinkView.File))
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Bewerk widget";
                return View("Edit", model);
            }

            File photoEntity = null;
            // Either upload file to AzureStorage or use file Key from explorer to get the file
            if (model.Image.NewUpload)
            {
                if (model.Image.UploadFile != null)
                {
                    photoEntity = FileUploadHelper.UploadFile(model.Image.UploadFile, FileUploadLocation.NewsWidget);
                }
            }
            else
            {
                if (model.Image.Key != "false")
                {
                    photoEntity = Db.Files.Single(m => m.Key == model.Image.Key);
                }
            }

            var link = singleWidgetItem.Target;
            if (model.LinkView.LinkType == "file" && model.LinkView.File.UploadFile != null || model.LinkView.File.Key != null && model.LinkView.File.Key != "false")
            {
                link = LinkManageHelper.SetLink(model.LinkView);
            }
            else if (model.LinkView.LinkType != "file")
            {
                link = LinkManageHelper.SetLink(model.LinkView);
            }

            singleWidgetItem.DisplayName = model.DisplayName;
            singleWidgetItem.EditedBy = User.Identity.Name;
            singleWidgetItem.Edited = DateTime.Now;
            singleWidgetItem.Status = model.Status;
            singleWidgetItem.Target = Db.Links.Single(m => m.Id == link.Id);
            if (photoEntity != null) singleWidgetItem.Image = Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: ButtonItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleWidgetItem = Db.ButtonItems.Find(id);

            singleWidgetItem.Order = null;
            singleWidgetItem.Status = false;
            singleWidgetItem.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: ButtonItems/SwitchStatus/5
        // Switch boolean Status from menuitem to opposite value
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleWidgetItem = Db.WidgetItems.Find(id);

            singleWidgetItem.Status = !singleWidgetItem.Status;
            switch (singleWidgetItem.Status)
            {
                case true:
                    singleWidgetItem.Order = Db.WidgetItems.OrderByDescending(m => m.Order).First().Order + 1;
                    break;
                case false:
                    singleWidgetItem.Order = null;
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
            var singleWidgetItem = Db.WidgetItems.Find(id);

            WidgetItem switchItem = null;
            switch (param)
            {
                case "up":
                    {
                        // Go to Manage if singleButtonItem already is the first item
                        if (Db.WidgetItems
                            .OrderBy(m => m.Order)
                            .First() == singleWidgetItem) return RedirectToAction("Manage");

                        switchItem = Db.WidgetItems
                            .Where(m => m.Order < singleWidgetItem.Order && m.Status)
                            .OrderByDescending(m => m.Order)
                            .FirstOrDefault();
                        break;
                    }
                case "down":
                    {
                        // Go to Manage if singleButtonItem already is the last item
                        // OrderDescending.First() is used because of issues from SQL limitations with the Last() method being used in this scenario
                        if (Db.WidgetItems
                            .OrderByDescending(m => m.Order)
                            .First() == singleWidgetItem) return RedirectToAction("Manage");

                        switchItem = Db.WidgetItems
                            .Where(m => m.Order > singleWidgetItem.Order && m.Status)
                            .OrderBy(m => m.Order)
                            .FirstOrDefault();
                        break;
                    }
            }
            if (switchItem == null || singleWidgetItem.Deleted) return RedirectToAction("Manage");

            // Save order of the item to switch with in temporary variable
            var t = switchItem.Order;
            switchItem.Order = singleWidgetItem.Order;
            singleWidgetItem.Order = t;

            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<WidgetItem> GetWidgetItems(bool includeDeleted = false)
        {
            return includeDeleted ? Db.WidgetItems : Db.WidgetItems.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleWidgetItem = Db.WidgetItems.Find(id);
            return singleWidgetItem != null && !singleWidgetItem.Deleted;
        }
    }
}