using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bigrivers.Client.Backend.Controllers
{
    public class MenuItemController : Controller
    {
        // GET: MenuItem
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }

        // GET: MenuItem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MenuItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuItem/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MenuItem/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MenuItem/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
