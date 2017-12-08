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
    public class EndOfLifeDisposalsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EndOfLifeDisposals
        public ActionResult Index()
        {
            return View(db.T_EndOfLifeDisposals.ToList());
        }

        // GET: EndOfLifeDisposals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndOfLifeDisposal endOfLifeDisposal = db.T_EndOfLifeDisposals.Find(id);
            if (endOfLifeDisposal == null)
            {
                return HttpNotFound();
            }
            return View(endOfLifeDisposal);
        }

        // GET: EndOfLifeDisposals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EndOfLifeDisposals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,No,Year,DisposalDate,EndOfLifeReason,CreatedBy,DisposalCompany,DisposedOfBy")] EndOfLifeDisposal endOfLifeDisposal)
        {
            if (ModelState.IsValid)
            {
                db.T_EndOfLifeDisposals.Add(endOfLifeDisposal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(endOfLifeDisposal);
        }

        // GET: EndOfLifeDisposals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndOfLifeDisposal endOfLifeDisposal = db.T_EndOfLifeDisposals.Find(id);
            if (endOfLifeDisposal == null)
            {
                return HttpNotFound();
            }
            return View(endOfLifeDisposal);
        }

        // POST: EndOfLifeDisposals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,No,Year,DisposalDate,EndOfLifeReason,CreatedBy,DisposalCompany,DisposedOfBy")] EndOfLifeDisposal endOfLifeDisposal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(endOfLifeDisposal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(endOfLifeDisposal);
        }

        // GET: EndOfLifeDisposals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndOfLifeDisposal endOfLifeDisposal = db.T_EndOfLifeDisposals.Find(id);
            if (endOfLifeDisposal == null)
            {
                return HttpNotFound();
            }
            return View(endOfLifeDisposal);
        }

        // POST: EndOfLifeDisposals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EndOfLifeDisposal endOfLifeDisposal = db.T_EndOfLifeDisposals.Find(id);
            db.T_EndOfLifeDisposals.Remove(endOfLifeDisposal);
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
