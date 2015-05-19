using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Client.Helpers;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Controllers
{
    public class SponsorsController : BaseController
    {
        // GET: Artist/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Artist/
        public ActionResult Manage()
        {
            var sponsors = GetSponsors().ToList();
            var listSponsors = sponsors.Where(m => m.Status)
                .ToList();

            listSponsors.AddRange(sponsors.Where(m => !m.Status).ToList());
            ViewBag.listSponsors = listSponsors;

            ViewBag.Title = "Nieuws";
            return View("Manage");
        }

        public ActionResult Search(string id)
        {
            ViewBag.listSponsors = GetSponsors()
                .Where(m => m.Name.Contains(id))
                .ToList();

            ViewBag.Title = "Zoek Sponsoren";
            return View("Manage");
        }

        // GET: Artist/New
        public ActionResult New()
        {
            var viewModel = new SponsorViewModel
            {
                Status = true
            };

            ViewBag.Title = "Sponsor toevoegen";
            return View("Edit", viewModel);
        }

        // POST: Artist/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(SponsorViewModel viewModel, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Sponsor toevoegen";
                return View("Edit", viewModel);
            }

            File photoEntity;
            if (ImageHelper.IsSize(file, 200000) && ImageHelper.IsMimes(file, new[] { "image" }))
            {
                photoEntity = ImageHelper.UploadFile(file, "sponsor");
            }
            else
            {
                return RedirectToAction("Manage");
            }

            var singleSponsor = new Sponsor
            {
                Name = viewModel.Name,
                Url = viewModel.Url,
                Image = photoEntity.Key,
                Status = viewModel.Status
            };

            // Only add file to DB if it hasn't been uploaded before
            if (!Db.Files.Any(m => m.Md5 == photoEntity.Md5)) Db.Files.Add(photoEntity);
            Db.Sponsors.Add(singleSponsor);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("New");
            var singleSponsor = Db.Sponsors.Find(id);
            if (singleSponsor == null || singleSponsor.Deleted) return RedirectToAction("Manage");

            var model = new SponsorViewModel
            {
                Name = singleSponsor.Name,
                Url = singleSponsor.Url,
                Image = singleSponsor.Image,
                Status = singleSponsor.Status
            };

            ViewBag.Title = "Bewerk Sponsor";
            return View(model);
        }

        // POST: Artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SponsorViewModel viewModel, HttpPostedFileBase file)
        {
            var singleSponsor = Db.Sponsors.Find(id);

            File photoEntity = null;
            if (file != null)
            {
                if (ImageHelper.IsSize(file, 200000) && ImageHelper.IsMimes(file, new[] { "image" }))
                {
                    photoEntity = ImageHelper.UploadFile(file, "sponsor");
                }
            }

            singleSponsor.Name = viewModel.Name;
            singleSponsor.Url = viewModel.Url;
            singleSponsor.Status = viewModel.Status;
            if (photoEntity != null && !Db.Files.Any(m => m.Md5 == photoEntity.Md5)) singleSponsor.Image = photoEntity.Key;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Artist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);
            if (singleSponsor == null || singleSponsor.Deleted) return RedirectToAction("Manage");

            

            singleSponsor.Status = false;
            singleSponsor.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (id == null) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);
            if (singleSponsor == null || singleSponsor.Deleted) return RedirectToAction("Manage");

            singleSponsor.Status = !singleSponsor.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Sponsor> GetSponsors(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Sponsors : Db.Sponsors.Where(a => !a.Deleted);
        }

    }
}
