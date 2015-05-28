using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Client.Backend.Helpers;
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

            File photoEntity = null;
            if (file != null)
            {
                if (FileUploadHelper.IsSize(file, 200000) && FileUploadHelper.IsMimes(file, new[] { "image" }))
                {
                    photoEntity = FileUploadHelper.UploadFile(file, "sponsor");
                }
                else
                {
                    return RedirectToAction("Manage");
                }
            }
            
            var singleSponsor = new Sponsor
            {
                Name = viewModel.Name,
                Url = viewModel.Url,
                Image = Db.Files.Single(m => m.Md5 == photoEntity.Md5 && m.Container == photoEntity.Container),
                Status = viewModel.Status
            };

            Db.Sponsors.Add(singleSponsor);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);

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
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Sponsor toevoegen";
                return View("Edit", viewModel);
            }

            File photoEntity = null;
            if (file != null)
            {
                if (FileUploadHelper.IsSize(file, 200000) && FileUploadHelper.IsMimes(file, new[] { "image" }))
                {
                    photoEntity = FileUploadHelper.UploadFile(file, "sponsor");
                }
            }

            singleSponsor.Name = viewModel.Name;
            singleSponsor.Url = viewModel.Url;
            singleSponsor.Status = viewModel.Status;
            if (photoEntity != null && !Db.Files.Any(m => m.Md5 == photoEntity.Md5)) singleSponsor.Image = Db.Files.Single(m => m.Md5 == photoEntity.Md5 && m.Container == photoEntity.Container);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Artist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);

            singleSponsor.Status = false;
            singleSponsor.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Artist/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleSponsor = Db.Sponsors.Find(id);

            singleSponsor.Status = !singleSponsor.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Sponsor> GetSponsors(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Sponsors : Db.Sponsors.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleSponsor = Db.Sponsors.Find(id);
            return singleSponsor != null && !singleSponsor.Deleted;
        }
    }
}
