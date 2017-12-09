﻿using System;
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
    public class AssetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ControllerVieBagHelper _controllerVieBagHelper = new ControllerVieBagHelper();

        // GET: Assets
        public ActionResult Index()
        {
            return View(db.T_Assets.ToList());
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

        //private void prepareViewBagDictionaryDescriptions(int assetLocationId, int assetTypeId, int depreciationTypeId)
        //{
        //    AssetLocation assetLocation = db.T_AssetLocations.Find(assetLocationId);
        //    AssetType assetType = db.T_AssetTypes.Find(assetTypeId);
        //    DepreciationType depreciationType = db.T_DepreciationTypes.Find(depreciationTypeId);

        //    ViewBag.AssetLocation = assetLocation.Name;
        //    ViewBag.AssetType = assetType.Name;
        //    ViewBag.DepreciationType = depreciationType.Name;
        //}

        //private void prepareViewBagDictionaryForEdit()
        //{
        //    ViewBag.AssetLocations = DataManipulation.GetAllItems(db.T_AssetLocations);
        //    ViewBag.AssetTypes = DataManipulation.GetAllItems(db.T_AssetTypes);
        //    ViewBag.DepreciationTypes = DataManipulation.GetAllItems(db.T_DepreciationTypes);
        //}

        //private void PopulateLocationsDropDownList(object selectedLocation = null)
        //{
        //    var locationQuery = from d in db.T_AssetLocations
        //                        orderby d.Name
        //                        select d;
        //    ViewBag.LocationId = new SelectList(locationQuery, "Id", "LocationName", selectedLocation);
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
