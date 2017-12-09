using FixedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FixedAssets.Controllers.Components
{
    public class ControllerVieBagHelper
    {
        public void PrepareViewBagAssetDictionaryDescriptions(Controller controller, ApplicationDbContext db, Asset asset)
        {
            AssetLocation assetLocation = db.T_AssetLocations.Find(asset.AssetLocationId);
            AssetType assetType = db.T_AssetTypes.Find(asset.AssetTypeId);
            DepreciationType depreciationType = db.T_DepreciationTypes.Find(asset.DepreciationTypeId);

            controller.ViewBag.AssetLocation = assetLocation.Name;
            controller.ViewBag.AssetType = assetType.Name;
            controller.ViewBag.DepreciationType = depreciationType.Name;
        }

        public void PrepareViewBagDictionaryForEdit(Controller controller, ApplicationDbContext db)
        {
            controller.ViewBag.AssetLocations = DataManipulation.GetAllItems(db.T_AssetLocations);
            controller.ViewBag.AssetTypes = DataManipulation.GetAllItems(db.T_AssetTypes);
            controller.ViewBag.DepreciationTypes = DataManipulation.GetAllItems(db.T_DepreciationTypes);
        }
    }
}