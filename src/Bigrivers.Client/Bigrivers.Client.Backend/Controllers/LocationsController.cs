using System.Linq;
using System.Web.Mvc;
using Bigrivers.Server.Model;
using Bigrivers.Client.Backend.ViewModels;

namespace Bigrivers.Client.Backend.Controllers
{
    public class LocationsController : BaseController
    {
        // GET: Performances/Index
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Performances/
        public ActionResult Manage()
        {
            var performances = GetLocations();
            var model = performances
                .Where(m => m.Status)
                .ToList();

            model.AddRange(performances
                .Where(m => !m.Status));

            ViewBag.Title = "Podia";
            return View(model);
        }

        // GET: Performances/Create
        public ActionResult New()
        {
            var model = new LocationViewModel
            {
                Status = true
            };

            ViewBag.Title = "Nieuw podium";
            return View("Edit", model);
        }

        // POST: Performances/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(LocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Nieuw podium";
                return View("Edit", model);
            }

            var singleLocation = new Location
            {
                Stagename = model.Stagename,
                City = model.City,
                Street = model.Street,
                Zipcode = model.Zipcode
            };

            Db.Locations.Add(singleLocation);
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Performances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleLocation = Db.Locations.Find(id);

            var model = new LocationViewModel
            {
                Stagename = singleLocation.Stagename,
                City = singleLocation.City,
                Street = singleLocation.Street,
                Zipcode = singleLocation.Zipcode
            };

            // Set all active parents into new list first

            ViewBag.Title = "Bewerk podium";
            return View(model);
        }

        // POST: Performances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Bewerk podium";
                return View("Edit", model);
            }

            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleLocation = Db.Locations.Find(id);

            singleLocation.Stagename = model.Stagename;
            singleLocation.City = model.City;
            singleLocation.Street = model.Street;
            singleLocation.Zipcode = model.Zipcode;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // POST: Performances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleLocation = Db.Locations.Find(id);

            singleLocation.Status = false;
            singleLocation.Deleted = true;
            Db.SaveChanges();

            return RedirectToAction("Manage");
        }

        // GET: Performances/SwitchStatus/5
        public ActionResult SwitchStatus(int? id)
        {
            if (!VerifyId(id)) return RedirectToAction("Manage");
            var singleLocation = Db.Locations.Find(id);

            singleLocation.Status = !singleLocation.Status;
            Db.SaveChanges();
            return RedirectToAction("Manage");
        }

        private IQueryable<Location> GetLocations(bool includeDeleted = false)
        {
            return includeDeleted ? Db.Locations : Db.Locations.Where(a => !a.Deleted);
        }

        private bool VerifyId(int? id)
        {
            if (id == null) return false;
            var singleLocation = Db.Locations.Find(id);
            return singleLocation != null && !singleLocation.Deleted;
        }
    }
}
