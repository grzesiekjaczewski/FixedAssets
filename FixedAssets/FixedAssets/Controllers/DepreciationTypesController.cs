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
    public class DepreciationTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DepreciationTypes
        public ActionResult Index()
        {
            return View(db.T_DepreciationTypes.ToList());
        }

        // GET: DepreciationTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepreciationType depreciationType = db.T_DepreciationTypes.Find(id);
            if (depreciationType == null)
            {
                return HttpNotFound();
            }
            return View(depreciationType);
        }

        // GET: DepreciationTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepreciationTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DepreciationTypeName,DepreciationRate")] DepreciationType depreciationType)
        {
            if (ModelState.IsValid)
            {
                db.T_DepreciationTypes.Add(depreciationType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depreciationType);
        }

        // GET: DepreciationTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepreciationType depreciationType = db.T_DepreciationTypes.Find(id);
            if (depreciationType == null)
            {
                return HttpNotFound();
            }
            return View(depreciationType);
        }

        // POST: DepreciationTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DepreciationTypeName,DepreciationRate")] DepreciationType depreciationType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depreciationType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depreciationType);
        }

        // GET: DepreciationTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepreciationType depreciationType = db.T_DepreciationTypes.Find(id);
            if (depreciationType == null)
            {
                return HttpNotFound();
            }
            return View(depreciationType);
        }

        // POST: DepreciationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DepreciationType depreciationType = db.T_DepreciationTypes.Find(id);
            db.T_DepreciationTypes.Remove(depreciationType);
            db.SaveChanges();
            return RedirectToAction("Index");
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
