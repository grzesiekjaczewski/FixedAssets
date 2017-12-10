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
            Depretiation depretiation = new Depretiation();
            Dictionary<int, string> monthNames = DataManipulation.GetMonthNames(db);
            Dictionary<int, DepreciationType> depreciationTypes = DataManipulation.GetDepreciationTypes(db);
            List<Asset> assetList = DataManipulation.GetAssetList(db);
            DepretiationPlanList depretiationPlanList = depretiation.CalculatePlan(startMonth, startYear, endMonth, endYear, monthNames);
            depretiation.CalculatePlanForAssets(depretiationPlanList, assetList, depreciationTypes);

            return View(depretiationPlanList);
        }
      
        //public ActionResult DepreciationPlan()
        //{
        //    Depretiation depretiation = new Depretiation();
        //    DepretiationPlanList depretiationPlanList = depretiation.CalculatePlan(db);
        //    return View(depretiationPlanList);
        //}



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