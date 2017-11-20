using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FixedAssets.Models
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        public string AssetName { get; set; }
        public List<AssetType> AssetTypes { get; set; }
        public bool IsUsed { get; set; }
    }

    public class AssetType
    {
        [Key]
        public int Id { get; set; }
        public string AssetTypeName { get; set; }
    }
}