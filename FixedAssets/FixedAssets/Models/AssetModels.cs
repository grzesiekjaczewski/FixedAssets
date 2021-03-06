﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FixedAssets.Models
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nazwa")]
        public string AssetName { get; set; }
        [Display(Name = "Nr inwent.")]
        public string InventoryNo { get; set; }
        [Display(Name = "Dowód zakupu")]
        public string ProofOfPurchase { get; set; }
        [Display(Name = "Przyjęto")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartUsingDate { get; set; }
        [Display(Name = "Wartość począt.")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal InitialValue { get; set; }
        [Display(Name = "Wartość umorzenia")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal AmortisedValue { get; set; }
        public List<DepreciationCharge> DepreciationCharges { get; set; }
        public List<ChangeInValue> ChangeInValues { get; set; }
        public List<EndOfLifeDisposal> EndOfLifeDisposals { get; set; }
        [Display(Name = "Zamortyzowany")]
        public bool Depreciated { get; set; }
        [Display(Name = "W użyciu")]
        public bool IsUsed { get; set; }
        public int DepreciationTypeId { get; set; }
        public int AssetTypeId { get; set; }
        public int AssetLocationId { get; set; }
        [NotMapped]
        [Display(Name = "Pozostało")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal RemainingAmount { get; set; }
    }

    public class AssetType : IDbDictionary
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Czy wyposażenie")]
        public bool LowValueAsset { get; set; }
        public List<Asset> Assets { get; set; }
    }

    public class AssetLocation : IDbDictionary
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        public List<Asset> Assets { get; set; }
    }

    public class DepreciationType : IDbDictionary
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Stawka %")]
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
        public int AssetId { get; set; }
        public int ReasonForChangingId { get; set; }
    }

    public class ChangeInValue
    {
        [Key]
        public int Id { get; set; }
        public DateTime ChangingDate { get; set; }
        public decimal ValueOfChange { get; set; }
        public decimal ValueAfterChange { get; set; }
        public int AssetId { get; set; }
        public int ReasonForChangingId { get; set; }
    }

    public class ReasonForChanging
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        public List<ChangeInValue> ChangeInValues { get; set; }
    }

    public class EndOfLifeDisposal
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Numer")]
        public int No { get; set; }
        public int Year { get; set; }
        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DisposalDate { get; set; }
        [Display(Name = "Przyczyna utylizacji")]
        public string EndOfLifeReason { get; set; }
        [Display(Name = "Utworzono przez")]
        public string CreatedBy { get; set; }
        [Display(Name = "Podmiot wykonujący utylizację")]
        public string DisposalCompany { get; set; }
        public string DisposedOfBy { get; set; }
        public int AssetId { get; set; }
    }

    public class MonthName
    {
        [Key]
        public int Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
    }

}