using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FixedAssets.Models;

namespace FixedAssets.Controllers
{
    [Authorize]
    [Authorize(Roles = "Supervisor")]
    public class AssetLocationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssetLocations
        public ActionResult Index()
        {
            return View(db.T_AssetLocations.ToList());
        }

        // GET: AssetLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetLocation assetLocation = db.T_AssetLocations.Find(id);
            if (assetLocation == null)
            {
                return HttpNotFound();
            }
            return View(assetLocation);
        }

        // GET: AssetLocations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssetLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AssetLocation assetLocation)
        {
            if (ModelState.IsValid)
            {
                db.T_AssetLocations.Add(assetLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assetLocation);
        }

        // GET: AssetLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetLocation assetLocation = db.T_AssetLocations.Find(id);
            if (assetLocation == null)
            {
                return HttpNotFound();
            }
            return View(assetLocation);
        }

        // POST: AssetLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AssetLocation assetLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assetLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assetLocation);
        }

        // GET: AssetLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetLocation assetLocation = db.T_AssetLocations.Find(id);
            if (assetLocation == null)
            {
                return HttpNotFound();
            }

            if (!DataManipulation.CannDeleteAssetLocation(db, id))
            {
                return RedirectToAction("CanNotDelete");
            }

            return View(assetLocation);
        }

        // POST: AssetLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssetLocation assetLocation = db.T_AssetLocations.Find(id);
            db.T_AssetLocations.Remove(assetLocation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CanNotDelete()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
