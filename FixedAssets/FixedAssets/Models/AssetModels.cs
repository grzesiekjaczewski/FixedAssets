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
        public int Quantity { get; set; }
        public List<AssetLocation> AssetLocations { get; set; }
        public DateTime StartUsingDate { get; set; }
        public decimal InitialValue { get; set; }
        public List<DepreciationType> DepreciationTypes { get; set; }
        public bool Depreciationeted { get; set; }
        public bool IsUsed { get; set; }
    }

    public class AssetType
    {
        [Key]
        public int Id { get; set; }
        public string AssetTypeName { get; set; }
    }

    public class AssetLocation
    {
        [Key]
        public int Id { get; set; }
        public string LocationName { get; set; }
    }

    public class DepreciationType
    {
        [Key]
        public int Id { get; set; }
        public string DepreciationTypeName { get; set; }
        public decimal Percent { get; set; }
    }
}