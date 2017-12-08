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
    }
}