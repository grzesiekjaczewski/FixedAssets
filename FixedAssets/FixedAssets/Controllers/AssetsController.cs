using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FixedAssets.Models;
using FixedAssets.Controllers.Components;

namespace FixedAssets.Controllers
{
    [Authorize]
    [Authorize(Roles = "Employee, Supervisor")]
    public class AssetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ControllerVieBagHelper _controllerVieBagHelper = new ControllerVieBagHelper();

        // GET: Assets
        public ActionResult Index()
        {
            return View(db.T_Assets.OrderBy(a => a.AssetName).ToList());
        }

        // GET: Assets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.T_Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            _controllerVieBagHelper.PrepareViewBagAssetDictionaryDescriptions(this, db, asset);
            return View(asset);
        }

        // GET: Assets/Create
        public ActionResult Create()
        {
            _controllerVieBagHelper.PrepareViewBagDictionaryForEdit(this, db);
            return View();
        }

        // POST: Assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AssetName,InventoryNo,ProofOfPurchase,StartUsingDate,InitialValue,AmortisedValue,Depreciated,IsUsed,DepreciationTypeId,AssetTypeId,AssetLocationId")] Asset asset, FormCollection forms)
        {
            var startUsingDate = Request["StartUsingDate1"];
            asset.StartUsingDate = Logic.CalculateDate.StringToDate(startUsingDate, ".", "/", "-");
            asset.IsUsed = true;
            asset.AmortisedValue = 0;
            asset.Depreciated = false;

            if (ModelState.IsValid)
            {
                db.T_Assets.Add(asset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(asset);
        }

        // GET: Assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.T_Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            _controllerVieBagHelper.PrepareViewBagDictionaryForEdit(this, db);

            return View(asset);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AssetName,InventoryNo,ProofOfPurchase,StartUsingDate,InitialValue,AmortisedValue,Depreciated,IsUsed,DepreciationTypeId,AssetTypeId,AssetLocationId")] Asset asset, FormCollection forms)
        {
            var startUsingDate = Request["StartUsingDate1"];
            asset.StartUsingDate = Logic.CalculateDate.StringToDate(startUsingDate, ".", "/", "-");
            _controllerVieBagHelper.PrepareViewBagDictionaryForEdit(this, db);

            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asset);
        }

        // GET: Assets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.T_Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }

            if (!DataManipulation.CannDeleteAsset(db, id))
            {
                return RedirectToAction("CanNotDelete");
            }

            _controllerVieBagHelper.PrepareViewBagAssetDictionaryDescriptions(this, db, asset);
            return View(asset);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asset asset = db.T_Assets.Find(id);
            db.T_Assets.Remove(asset);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CanNotDelete()
        {
            return View();
        }


        // GET: Assets/Details/5
        public ActionResult EndOfLifeDisposal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.T_Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            
            return View(asset);
        }

        // GET: Assets/Details/5
        public ActionResult ModyfyInitialValue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.T_Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }

            return View(asset);
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
