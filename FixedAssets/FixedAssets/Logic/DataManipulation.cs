using FixedAssets.Logic;
using FixedAssets.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Web.WebPages.Html;

namespace FixedAssets
{
    static public class DataManipulation
    {
        static public IEnumerable<SelectListItem> GetAllItems<TEntity>(DbSet<TEntity> dbTable) where TEntity : class, IDbDictionary
        {
            IEnumerable<SelectListItem> items = dbTable.OrderBy(n => n.Name).Select(c => new SelectListItem
            { Value = c.Id.ToString(),
              Text = c.Name
            });
            return items;
        }

        static public bool CannDeleteAsset(ApplicationDbContext db, int? assetId)
        {
            if (assetId == null) assetId = 0;
            bool test = true;
            if (db.T_DepreciationCharges.Where(dc => dc.AssetId == assetId).Count() > 0)
            {
                test = false;
            }
            return test;
        }

        static public List<ChangeInValueItem> GetChangeInValueList(ApplicationDbContext db, int? assetId)
        {
            if (assetId == null) assetId = 0;
            List<ChangeInValue> changeInValues = db.T_ChangeInValues.Where(c => c.AssetId == assetId).ToList();

            List<ChangeInValueItem> changeInValueItems = new List<ChangeInValueItem>();

            foreach(ChangeInValue changeInValue in changeInValues)
            {
                ChangeInValueItem changeInValueItem = new ChangeInValueItem();
                changeInValueItem.ChangingDate = changeInValue.ChangingDate;
                ReasonForChanging reasonForChanging = db.T_ReasonForChangings.Where(r => r.Id == changeInValue.ReasonForChangingId).FirstOrDefault();
                changeInValueItem.ReasonForChangingName = reasonForChanging.Description;
                changeInValueItem.ValueOfChange = changeInValue.ValueOfChange;
                changeInValueItem.ValueAfterChange = changeInValue.ValueAfterChange;
                changeInValueItems.Add(changeInValueItem);
            }

            return changeInValueItems;
        }

        static public bool CannDeleteAssetLocation(ApplicationDbContext db, int? assetLocationId)
        {
            if (assetLocationId == null) assetLocationId = 0;
            bool test = true;
            if (db.T_Assets.Where(a => a.AssetLocationId == assetLocationId).Count() > 0)
            {
                test = false;
            }
            return test;
        }

        static public bool CannDeleteAssetType(ApplicationDbContext db, int? assetTypeId)
        {
            if (assetTypeId == null) assetTypeId = 0;
            bool test = true;
            if (db.T_Assets.Where(a => a.AssetTypeId == assetTypeId).Count() > 0)
            {
                test = false;
            }
            return test;
        }

        static public bool CannDeleteDepreciationType(ApplicationDbContext db, int? depreciationTypeId)
        {
            if (depreciationTypeId == null) depreciationTypeId = 0;
            bool test = true;
            if (db.T_Assets.Where(a => a.DepreciationTypeId == depreciationTypeId).Count() > 0)
            {
                test = false;
            }
            return test;
        }

        static public bool CannDeleteReasonForChangings(ApplicationDbContext db, int? reasonForChangingId)
        {
            if (reasonForChangingId == null) reasonForChangingId = 0;
            bool test = true;
            if (db.T_ChangeInValues.Where(a => a.ReasonForChangingId == reasonForChangingId).Count() > 0)
            {
                test = false;
            }
            return test;
        }

        static public void GetEndOfLifeDisposal(Controller controller, ApplicationDbContext db, int? assetId)
        {
            if (db.T_EndOfLifeDisposals.Where(e => e.AssetId == assetId).Count() == 0)
            {
                controller.ViewBag.EndOfLifeDisposal1 = "";
                return;
            }
            EndOfLifeDisposal endOfLifeDisposal = db.T_EndOfLifeDisposals.Where(e => e.AssetId == assetId).FirstOrDefault();
            controller.ViewBag.EndOfLifeDisposal1 = "Dyspozycja utylizacji:";
            controller.ViewBag.EndOfLifeDisposal2 = endOfLifeDisposal.No.ToString("000") + "/" + endOfLifeDisposal.Year.ToString();
            controller.ViewBag.EndOfLifeDisposal3 = endOfLifeDisposal.DisposalDate.ToString("dd.MM.yyyy");
            controller.ViewBag.EndOfLifeDisposal4 = endOfLifeDisposal.EndOfLifeReason;
            controller.ViewBag.EndOfLifeDisposal5 = endOfLifeDisposal.CreatedBy;
            controller.ViewBag.EndOfLifeDisposal6 = endOfLifeDisposal.DisposalCompany;
        }



        static public Dictionary<int, string> GetMonthNames(ApplicationDbContext db)
        {
            return db.T_MonthNames.Select(m => new { m.No, m.Name }).ToDictionary(m => m.No, m => m.Name);
        }

        static public List<Asset> GetAssetList(ApplicationDbContext db)
        {
            return db.T_Assets.ToList();
        }

        static public List<AssetType> GetAssetTypes(ApplicationDbContext db)
        {
            return db.T_AssetTypes.ToList();
        }

        static public DepreciationType GetDepreciationTypeForAsset(ApplicationDbContext db, int depreciationTypeId)
        {
            return db.T_DepreciationTypes.Where(dt => dt.Id == depreciationTypeId).FirstOrDefault<DepreciationType>();
        }

        static public Dictionary<int, DepreciationType> GetDepreciationTypes(ApplicationDbContext db)
        {
            return db.T_DepreciationTypes.Select(dt => new { dt.Id, dt }).ToDictionary(dt => dt.Id, dt => dt.dt);
        }

        static public Dictionary<string, DepreciationCharge> GetDepreciationCharges(ApplicationDbContext db)
        {
            return db.T_DepreciationCharges.Select(dc => new { dc.Year, dc.Month, dc.AssetId, dc })
                .ToDictionary(dc => dc.Year.ToString() + dc.Month.ToString("00") + dc.AssetId.ToString(), dc => dc.dc);
        }

        static public List<DepreciationCharge> GetAssetDepreciationCharges(ApplicationDbContext db, int assetId)
        {
            return db.T_DepreciationCharges.Where(dc => dc.AssetId == assetId).OrderBy(dc => dc.Year).ThenBy(dc => dc.Month).ToList();
        }

        static public bool CanProcessDepretiatin(DateTime prevDate, DateTime nextDate)
        {
            if (prevDate.Year < nextDate.Year) return true;
            if (prevDate.Year == nextDate.Year && prevDate.Month < nextDate.Month) return true;
            return false;
        }
    }
}