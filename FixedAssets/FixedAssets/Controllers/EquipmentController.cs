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
    public class EquipmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Equipment
        public ActionResult Index()
        {
            List<AssetType> assetTypes = db.T_AssetTypes.OrderBy(a => a.Name).ToList();
            List<Asset> assets = db.T_Assets.OrderBy(a => a.AssetName).ToList();

            Equipment equipment = new Equipment();
            List<EquipmentElement> equipmentElements = equipment.PrepareEquipment(assets, assetTypes);

            return View(equipmentElements);
        }

        // GET: Assets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Asset> equipmentAssets = db.T_Assets.Where(a => a.AssetTypeId == id && a.IsUsed == true).ToList();
            foreach(Asset asset in equipmentAssets) { asset.RemainingAmount = asset.InitialValue - asset.AmortisedValue; }

            return View(equipmentAssets);
        }
    }
}