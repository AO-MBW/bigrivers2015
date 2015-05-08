using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class NewsController : BaseController
    {
        // GET: Artist/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Artist/
        public ActionResult Manage()
        {
            var newsItems = GetNewsItems().ToList();
            var listNewsItems = newsItems.Where(m => m.Status)
                .ToList();

            listNewsItems.AddRange(newsItems.Where(m => !m.Status).ToList());
            ViewBag.listNewsItems = listNewsItems;

            ViewBag.Title = "Nieuws";
            return View("Manage");
        }

        public ActionResult Search(string id)
        {
            ViewBag.listNewsItems = GetNewsItems()
                .Where(m => m.Title.Contains(id))
                .ToList();

            ViewBag.Title = "Zoek Nieuws";
            return View("Manage");
        }

        // GET: Artist/New
        public ActionResult New()
        {
            var viewModel = new NewsItemViewModel
            {
                Status = true
            };

            ViewBag.Title = "Nieuws toevoegen";
            return View("Edit", viewModel);
        }

        // POST: Artist/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(NewsItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuws toevoegen";
                return View("Edit", viewModel);
            }

            var singleNewsItem = new NewsItem
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                Image = viewModel.Image,
                Status = viewModel.Status
            };

            Db.NewsItems.Add(singleNewsItem);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");
            var singleNewsItem = Db.NewsItems.Find(id);
            if (singleNewsItem == null || singleNewsItem.Deleted) return RedirectToAction("Manage");

            var model = new NewsItemViewModel
            {
                Title = singleNewsItem.Title,
                Content = singleNewsItem.Content,
                Image = singleNewsItem.Image,
                Status = singleNewsItem.Status
            };

            ViewBag.Title = "Bewerk Nieuws";
            return View(model);
        }

        // POST: Artist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, NewsItemViewModel viewModel)
        {
            var singleNewsItem = Db.NewsItems.Find(id);

            singleNewsItem.Title = viewModel.Title;
            singleNewsItem.Content = viewModel.Content;
            singleNewsItem.Image = viewModel.Image;
            singleNewsItem.Status = viewModel.Status;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Artist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singleNewsItem = Db.NewsItems.Find(id);
            if (singleNewsItem == null || singleNewsItem.Deleted) return RedirectToAction("Manage");

            singleNewsItem.Status = false;
            singleNewsItem.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singleNewsItem = Db.NewsItems.Find(id);
            if (singleNewsItem == null || singleNewsItem.Deleted) return RedirectToAction("Manage");

            singleNewsItem.Status = !singleNewsItem.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<NewsItem> GetNewsItems(bool includeDeleted = false)
        {
            return includeDeleted ? Db.NewsItems : Db.NewsItems.Where(a => !a.Deleted);
        }

    }
}
