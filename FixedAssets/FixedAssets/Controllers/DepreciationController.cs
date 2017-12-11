using FixedAssets.Controllers.Components;
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
    [Authorize]
    [Authorize(Roles = "Employee, Supervisor")]
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
            asset.RemainingAmount = asset.InitialValue - asset.AmortisedValue;
            _controllerVieBagHelper.PrepareViewBagAssetDictionaryDescriptions(this, db, asset);
            return View(asset);
        }


        public ActionResult DepreciationPlanParameters()
        {
            PrepareYearMonths prepareYearMonths = new PrepareYearMonths();
            YearMonths yearMonths = prepareYearMonths.GetYearMonths();
            yearMonths.EndYear = DateTime.Now.Year + 1;

            return View(yearMonths);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepreciationPlanParameters([Bind(Include = "StartMonth, StartYear, EndMonth, EndYear")] YearMonths yearMonths)
        {
            return RedirectToAction("DepreciationPlan", new {
                startMonth = yearMonths.StartMonth,
                startYear = yearMonths.StartYear,
                endMonth = yearMonths.EndMonth,
                endYear = yearMonths.EndYear });
        }

        public ActionResult DepreciationPlan(int startMonth, int startYear, int endMonth, int endYear)
        {
            Depreciation depreciation = new Depreciation();
            MyDataSet myDataSet = new MyDataSet(db);
            DepreciationPlanList depreciationPlanList = depreciation.CalculatePlan(startMonth, startYear, endMonth, endYear, myDataSet.MonthNames);
            depreciation.CalculatePlanForAssets(depreciationPlanList, myDataSet, false);

            return View(depreciationPlanList);
        }
      
        public ActionResult DepreciationParameters()
        {
            PrepareYearMonths prepareYearMonths = new PrepareYearMonths();
            YearMonths yearMonths = prepareYearMonths.GetYearMonths();
            yearMonths.SetForDepreciation();

            return View(yearMonths);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepreciationParameters([Bind(Include = "EndMonth, EndYear")] YearMonths yearMonths)
        {
            return RedirectToAction("Depreciation", new
            {
                startMonth = 1,
                startYear = 2015,
                endMonth = yearMonths.EndMonth,
                endYear = yearMonths.EndYear
            });
        }

        public ActionResult Depreciation(int startMonth, int startYear, int endMonth, int endYear)
        {
            Depreciation depreciation = new Depreciation();
            MyDataSet myDataSet = new MyDataSet(db);
            DepreciationPlanList depreciationPlanList = depreciation.CalculatePlan(startMonth, startYear, endMonth, endYear, myDataSet.MonthNames);
            depreciation.CalculatePlanForAssets(depreciationPlanList, myDataSet, true);

            return View(depreciationPlanList);
        }

        public ActionResult DepreciationViewParameters()
        {
            PrepareYearMonths prepareYearMonths = new PrepareYearMonths();
            YearMonths yearMonths = prepareYearMonths.GetYearMonths();
            yearMonths.SetForDepreciationView();
            return View(yearMonths);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepreciationViewParameters([Bind(Include = "StartMonth, StartYear, EndMonth, EndYear")] YearMonths yearMonths)
        {
            return RedirectToAction("DepreciationView", new
            {
                startMonth = yearMonths.StartMonth,
                startYear = yearMonths.StartYear,
                endMonth = yearMonths.EndMonth,
                endYear = yearMonths.EndYear
            });
        }

        public ActionResult DepreciationView(int startMonth, int startYear, int endMonth, int endYear)
        {
            Depreciation depreciation = new Depreciation();
            MyDataSet myDataSet = new MyDataSet(db);
            DepreciationPlanList depreciationPlanList = depreciation.CalculatePlan(startMonth, startYear, endMonth, endYear, myDataSet.MonthNames);
            depreciation.CalculateProcessedDepreciation(depreciationPlanList, myDataSet);

            return View(depreciationPlanList);
        }

        public ActionResult AssetDepreciationPlan(int? id)
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

            Depreciation depreciation = new Depreciation();
            AssetDataSet assetDataSet = new AssetDataSet(db, asset.Id);
            AssetDepreciationPlan assetDepreciationPlan = depreciation.CalculateAssetDepreciationPlan(assetDataSet, asset, true);

            return View(assetDepreciationPlan);
        }


        public ActionResult AssetDepreciationView(int? id)
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

            Depreciation depreciation = new Depreciation();
            AssetDataSet assetDataSet = new AssetDataSet(db, asset.Id);
            AssetDepreciationPlan assetDepreciationPlan = depreciation.CalculateAssetDepreciationPlan(assetDataSet, asset, false);

            return View(assetDepreciationPlan);
        }

    }

    public class AssetDataSet
    {
        public AssetDataSet() {}

        public AssetDataSet(ApplicationDbContext db, int assetId)
        {
            MonthNames = DataManipulation.GetMonthNames(db);
            DepreciationTypes = DataManipulation.GetDepreciationTypes(db);
            DepreciationCharges = DataManipulation.GetAssetDepreciationCharges(db, assetId);
        }

        public Dictionary<int, string> MonthNames;
        public Dictionary<int, DepreciationType> DepreciationTypes;
        public List<DepreciationCharge> DepreciationCharges;
    }

    public class MyDataSet
    {
        public MyDataSet() {}

        public MyDataSet(ApplicationDbContext db)
        {
            MonthNames = DataManipulation.GetMonthNames(db);
            DepreciationTypes = DataManipulation.GetDepreciationTypes(db);
            AssetList = DataManipulation.GetAssetList(db);
            DepreciationCharges = DataManipulation.GetDepreciationCharges(db);
        }

        public Dictionary<int, string> MonthNames;
        public Dictionary<int, DepreciationType> DepreciationTypes;
        public List<Asset> AssetList;
        public Dictionary<string, DepreciationCharge> DepreciationCharges;
    }

}