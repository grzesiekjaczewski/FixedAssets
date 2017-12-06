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
        public string InventoryNo { get; set; }
        public string ProofOfPurchase { get; set; }
        public List<AssetLocation> AssetLocations { get; set; }
        public DateTime StartUsingDate { get; set; }
        public decimal InitialValue { get; set; }
        public decimal AmortisedValue { get; set; }
        public List<DepreciationCharge> DepreciationCharges { get; set; }
        public List<ChangeInValue> ChangeInValues { get; set; }
        public List<EndOfLifeDisposal> EndOfLifeDisposals { get; set; }
        public bool Depreciated { get; set; }
        public bool IsUsed { get; set; }
    }

    public class AssetType
    {
        [Key]
        public int Id { get; set; }
        public string AssetTypeName { get; set; }
        public bool LowValueAsset { get; set; }
        public List<Asset> Assets { get; set; }
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
        public decimal DepreciationRate { get; set; }
        public List<Asset> Assets { get; set; }
    }

    public class DepreciationCharge
    {
        [Key]
        public int Id { get; set; }
        public int No { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public decimal CurrentCharge { get; set; }
        public decimal CumulativelyCharge { get; set; }
        public decimal RemainingAmount { get; set; }
    }

    public class ChangeInValue
    {
        [Key]
        public int Id { get; set; }
        public DateTime ChangingDate { get; set; }
        public decimal ValueOfChange { get; set; }
        public decimal ValueAfterChange { get; set; }
    }

    public class ReasonForChanging
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public List<ChangeInValue> ChangeInValues { get; set; }
    }

    public class EndOfLifeDisposal
    {
        [Key]
        public int Id { get; set; }
        public int No { get; set; }
        public int Year { get; set; }
        public DateTime DisposalDate { get; set; }
        public string EndOfLifeReason { get; set; }
        public string CreatedBy { get; set; }
        public string DisposalCompany { get; set; }
        public string DisposedOfBy { get; set; }
    }
}