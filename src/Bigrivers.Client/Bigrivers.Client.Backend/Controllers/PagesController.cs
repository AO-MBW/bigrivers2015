using System;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class PagesController : BaseController
    {
        // GET: News/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: News/
        public ActionResult Manage()
        {
            var pages = GetPages();
            var model = pages
                .Where(m => m.Status)
                .ToList();

            model.AddRange(pages
                .Where(m => !m.Status));

            ViewBag.Title = "Pagina's";
            return View(model);
        }

        // GET: News/New
        public ActionResult New()
        {
            var model = new PageViewModel
            {
                Status = true
            };

            ViewBag.Title = "Pagina aanmaken";
            return View("Edit", model);
        }

        // POST: News/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(PageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Pagina aanmaken";
                return View("Edit", model);
            }

            var singlePage = new Page
            {
                Title = model.Title,
                Content = model.Content,
                EditedBy = User.Identity.Name,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                Status = model.Status
            };

            Db.Pages.Add(singlePage);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singlePage = Db.Pages.Find(id);

            var model = new PageViewModel
            {
                Title = singlePage.Title,
                Content = singlePage.Content,
                Status = singlePage.Status
            };

            ViewBag.Title = "Bewerk pagina";
            return View(model);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PageViewModel model)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singlePage = Db.Pages.Find(id);

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Bewerk pagina";
                return View("Edit", model);
            }

            singlePage.Title = model.Title;
            singlePage.Content = model.Content;
            singlePage.EditedBy = User.Identity.Name;
            singlePage.Edited = DateTime.Now;
            singlePage.Status = model.Status;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singlePage = Db.Pages.Find(id);

            singlePage.Status = false;
            singlePage.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: News/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singlePage = Db.Pages.Find(id);

            singlePage.Status = !singlePage.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Page> GetPages(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Pages : Db.Pages.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singlePage = Db.Pages.Find(id);
            return singlePage != null && !singlePage.Deleted;
        }
    }
}
