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
    public class ReasonForChangingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ReasonForChangings
        public ActionResult Index()
        {
            return View(db.T_ReasonForChangings.ToList());
        }

        // GET: ReasonForChangings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReasonForChanging reasonForChanging = db.T_ReasonForChangings.Find(id);
            if (reasonForChanging == null)
            {
                return HttpNotFound();
            }
            return View(reasonForChanging);
        }

        // GET: ReasonForChangings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReasonForChangings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] ReasonForChanging reasonForChanging)
        {
            if (ModelState.IsValid)
            {
                db.T_ReasonForChangings.Add(reasonForChanging);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reasonForChanging);
        }

        // GET: ReasonForChangings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReasonForChanging reasonForChanging = db.T_ReasonForChangings.Find(id);
            if (reasonForChanging == null)
            {
                return HttpNotFound();
            }
            return View(reasonForChanging);
        }

        // POST: ReasonForChangings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] ReasonForChanging reasonForChanging)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reasonForChanging).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reasonForChanging);
        }

        // GET: ReasonForChangings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReasonForChanging reasonForChanging = db.T_ReasonForChangings.Find(id);
            if (reasonForChanging == null)
            {
                return HttpNotFound();
            }
            if (!DataManipulation.CannDeleteReasonForChangings(db, id))
            {
                return RedirectToAction("CanNotDelete");
            }
            return View(reasonForChanging);
        }

        // POST: ReasonForChangings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReasonForChanging reasonForChanging = db.T_ReasonForChangings.Find(id);
            db.T_ReasonForChangings.Remove(reasonForChanging);
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
