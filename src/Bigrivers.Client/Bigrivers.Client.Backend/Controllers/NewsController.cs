using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Helpers;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: News/
        public ActionResult Manage()
        {
            var newsItems = GetNewsItems();
            var model = newsItems
                .Where(m => m.Status)
                .ToList();

            model.AddRange(newsItems
                .Where(m => !m.Status));

            ViewBag.Title = "Nieuws";
            return View(model);
        }

        // GET: News/New
        public ActionResult New()
        {
            var model = new NewsItemViewModel
            {
                Publish = DateTime.Now,
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.News).ToList()
                },
                Status = true
            };

            ViewBag.Title = "Nieuws toevoegen";
            return View("Edit", model);
        }

        // POST: News/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(NewsItemViewModel model)
        {
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.News).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileUploadValidator.News.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuws toevoegen";
                return View("Edit", model);
            }

            File photoEntity = null;
            // Either upload file to AzureStorage or use file Key from explorer to get the file
            if (model.Image.NewUpload)
            {
                if (model.Image.UploadFile != null)
                {
                    photoEntity = FileUploadHelper.UploadFile(model.Image.UploadFile, FileUploadLocation.News);
                }
            }
            else
            {
                if (model.Image.Key != "false")
                {
                    photoEntity = Db.Files.Single(m => m.Key == model.Image.Key);
                }
            }

            var singleNewsItem = new NewsItem
            {
                Title = model.Title,
                Content = model.Content,
                Publish = model.Publish,
                EditedBy = User.Identity.Name,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                Status = model.Status,
                Image = photoEntity != null ? Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key) : null
            };

            Db.NewsItems.Add(singleNewsItem);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleNewsItem = Db.NewsItems.Find(id);

            var model = new NewsItemViewModel
            {
                Title = singleNewsItem.Title,
                Content = singleNewsItem.Content,
                Publish = singleNewsItem.Publish.DateTime,
                Image = new FileUploadViewModel
                {
                    NewUpload = true,
                    ExistingFile = singleNewsItem.Image,
                    FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.News).ToList()
                },
                Status = singleNewsItem.Status
            };

            ViewBag.Title = "Bewerk Nieuws";
            return View(model);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NewsItemViewModel model)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleNewsItem = Db.NewsItems.Find(id);

            model.Image.ExistingFile = singleNewsItem.Image;
            model.Image.FileBase = Db.Files.Where(m => m.Container == FileUploadLocation.News).ToList();

            // Run over a validator to add custom model errors
            foreach (var error in FileUploadValidator.News.CheckFile(model.Image))
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuws toevoegen";
                return View("Edit", model);
            }

            File photoEntity = null;
            // Either upload file to AzureStorage or use file Key from explorer to get the file
            if (model.Image.NewUpload)
            {
                if (model.Image.UploadFile != null)
                {
                    photoEntity = FileUploadHelper.UploadFile(model.Image.UploadFile, FileUploadLocation.News);
                }
            }
            else
            {
                if (model.Image.Key != "false")
                {
                    photoEntity = Db.Files.Single(m => m.Key == model.Image.Key);
                }
            }

            singleNewsItem.Title = model.Title;
            singleNewsItem.Content = model.Content;
            singleNewsItem.Publish = model.Publish;
            singleNewsItem.EditedBy = User.Identity.Name;
            singleNewsItem.Edited = DateTime.Now;
            singleNewsItem.Status = model.Status;
            if (photoEntity != null) singleNewsItem.Image = Db.Files.SingleOrDefault(m => m.Key == photoEntity.Key);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleNewsItem = Db.NewsItems.Find(id);

            singleNewsItem.Status = false;
            singleNewsItem.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: News/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleNewsItem = Db.NewsItems.Find(id);

            singleNewsItem.Status = !singleNewsItem.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<NewsItem> GetNewsItems(bool includeDeleted = false)
        {
            return includeDeleted ? Db.NewsItems : Db.NewsItems.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleNewsItem = Db.NewsItems.Find(id);
            return singleNewsItem != null && !singleNewsItem.Deleted;
        }
    }
}
