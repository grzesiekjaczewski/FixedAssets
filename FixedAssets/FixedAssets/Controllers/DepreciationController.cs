﻿using FixedAssets.Controllers.Components;
using FixedAssets.Logic;
using FixedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FixedAssets.Controllers
{
    public class DepreciationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ControllerVieBagHelper _controllerVieBagHelper = new ControllerVieBagHelper();

        // GET: Depreciation
        public ActionResult Index()
        {
            return View(db.T_Assets.ToList());
        }

        // GET: Depreciation/Details/5
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

        public ActionResult DepreciationPlanParameters()
        {
            PrepareYearMonths prepareYearMonths = new PrepareYearMonths();
            YearMonths yearMonths = prepareYearMonths.GetYearMonths();
            yearMonths.Year = DateTime.Now.Year + 1;

            return View(yearMonths);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepreciationPlanParameters([Bind(Include = "Month, Year")] YearMonths yearMonths)
        {
            return RedirectToAction("DepreciationPlan", new { month = yearMonths.Month, year = yearMonths.Year });
        }

        public ActionResult DepreciationPlan(int month, int year)
        {
            return View();
        }

        public ActionResult DepreciationParameters()
        {
            return View();
        }

        public ActionResult Depreciation()
        {
            return View();
        }

        public ActionResult DepreciationViewParameters()
        {
            return View();
        }

        public ActionResult DepreciationView()
        {
            return View();
        }

    }
}