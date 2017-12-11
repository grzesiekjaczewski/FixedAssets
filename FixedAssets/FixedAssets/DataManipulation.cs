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

        static public Dictionary<int, string> GetMonthNames(ApplicationDbContext db)
        {
            return db.T_MonthNames.Select(m => new { m.No, m.Name }).ToDictionary(m => m.No, m => m.Name);
        }

        static public List<Asset> GetAssetList(ApplicationDbContext db)
        {
            return db.T_Assets.Where(a => a.Depreciated == false).ToList();
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


        static public bool CanProcessDepretiatin(DateTime prevDate, DateTime nextDate)
        {
            if (prevDate.Year < nextDate.Year) return true;
            if (prevDate.Year == nextDate.Year && prevDate.Month < nextDate.Month) return true;
            return false;
        }
    }
}