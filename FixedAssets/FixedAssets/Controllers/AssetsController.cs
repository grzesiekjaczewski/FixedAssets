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
using FixedAssets.Logic;

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
            ViewBag.ChangeInValueList = DataManipulation.GetChangeInValueList(db, id);
            DataManipulation.GetEndOfLifeDisposal(this, db, id);
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
            DataManipulation.GetEndOfLifeDisposal(this, db, id);

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

            EndOfLifeDosposalItem endOfLifeDosposalItem = new EndOfLifeDosposalItem();

            endOfLifeDosposalItem.AssetId = asset.Id;
            endOfLifeDosposalItem.AssetName = asset.AssetName;
            endOfLifeDosposalItem.DisposalDate = DateTime.Now;
            endOfLifeDosposalItem.CreatedBy = User.Identity.Name;

            return View(endOfLifeDosposalItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EndOfLifeDisposal([Bind(Include = "AssetId,EndOfLifeReason,CreatedBy,DisposalCompany")] EndOfLifeDosposalItem endOfLifeDisposalItem)
        {
            Asset asset = db.T_Assets.Find(endOfLifeDisposalItem.AssetId);

            EndOfLifeDisposal endOfLifeDisposal = new Models.EndOfLifeDisposal();

            int no = 0;

            endOfLifeDisposal.Year = DateTime.Now.Year;
            endOfLifeDisposalItem.DisposalDate = DateTime.Now;

            if (db.T_EndOfLifeDisposals.Where(e => e.Year == endOfLifeDisposal.Year).Count() == 0)
            {
                no = 1;
            }
            else
            {
                no = db.T_EndOfLifeDisposals.Where(e => e.Year == endOfLifeDisposal.Year).Max(e => e.No);
                no++;
            }

            endOfLifeDisposal.No = no;
            endOfLifeDisposal.DisposalDate = endOfLifeDisposalItem.DisposalDate;
            endOfLifeDisposal.EndOfLifeReason = endOfLifeDisposalItem.EndOfLifeReason;
            endOfLifeDisposal.CreatedBy = endOfLifeDisposalItem.CreatedBy;
            endOfLifeDisposal.DisposalCompany = endOfLifeDisposalItem.DisposalCompany;
            endOfLifeDisposal.AssetId = endOfLifeDisposalItem.AssetId;

            asset.IsUsed = false;

            db.T_EndOfLifeDisposals.Add(endOfLifeDisposal);
            db.SaveChanges();

            return RedirectToAction("Index", "EndOfLifeDisposals");
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

            ChangeInValueItem changeInValueItem = new ChangeInValueItem();

            changeInValueItem.AssetId = asset.Id;
            changeInValueItem.AssetName = asset.AssetName;
            changeInValueItem.ChangingDate = DateTime.Now;
            changeInValueItem.ReasonForChangings = db.T_ReasonForChangings.ToList();
            changeInValueItem.ValueBeforeChange = asset.InitialValue;
            changeInValueItem.ValueAfterChange = asset.InitialValue;

            return View(changeInValueItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModyfyInitialValue([Bind(Include = "AssetId,ChangingDate,ValueOfChange,ValueAfterChange,ReasonForChangingId,AssetName,ChangingDate")] ChangeInValueItem changeInValueItem)
        {
            Asset asset = db.T_Assets.Find(changeInValueItem.AssetId);

            ChangeInValue changeInValue = new ChangeInValue();
            changeInValue.AssetId = changeInValueItem.AssetId;
            changeInValue.ChangingDate = changeInValueItem.ChangingDate;
            changeInValue.ReasonForChangingId = changeInValueItem.ReasonForChangingId;
            changeInValue.ValueAfterChange = changeInValueItem.ValueAfterChange;
            changeInValue.ValueOfChange = changeInValueItem.ValueAfterChange - asset.InitialValue;

            asset.InitialValue = changeInValueItem.ValueAfterChange;
            db.T_ChangeInValues.Add(changeInValue);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = changeInValueItem.AssetId });
        }

        //public ActionResult ChangeInValueList(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Asset asset = db.T_Assets.Find(id);
        //    if (asset == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    List<ChangeInValue> changeInValues = DataManipulation.GetChangeInValueList(db, id);

        //    return View(changeInValues);
        //}


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
