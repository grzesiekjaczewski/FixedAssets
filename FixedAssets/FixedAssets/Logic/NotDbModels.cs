using FixedAssets.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FixedAssets.Logic
{
    public class Month
    {
        public int No { get; set; }
        //public string Name { get; set; }
    }

    public class Year
    {
        public int No { get; set; }
    }


    public class YearMonths
    {
        public YearMonths()
        {
            EndMonths = new List<Logic.Month>();
            EndYears = new List<Logic.Year>();
            StartMonths = new List<Logic.Month>();
            StartYears = new List<Logic.Year>();
        }

        [Display(Name = "Miesiąc rozpoczęcia planu")]
        public int StartMonth { get; set; }
        [Display(Name = "Rok rozpoczęcia planu")]
        public int StartYear { get; set; }
        public List<Month> StartMonths { get; set; }
        public List<Year> StartYears { get; set; }

        [Display(Name = "Miesiąc zakończenia planu")]
        public int EndMonth { get; set; }
        [Display(Name = "Rok zakończenia planu")]
        public int EndYear { get; set; }
        public List<Month> EndMonths { get; set; }
        public List<Year> EndYears { get; set; }

        public void SetForDepreciation()
        {
            if (StartMonth > 1)
            {
                StartMonth--;
                EndMonth--;
            }
            else
            {
                StartMonth = 1;
                EndMonth = 1;
                StartYear--;
                EndYear--;
            }
        }

        public void SetForDepreciationView()
        {
            StartMonth = 1;
        }
    }

    public class PrepareYearMonths
    {
        public YearMonths GetYearMonths()
        {
            YearMonths yearMonths = new YearMonths();

            yearMonths.EndYear = DateTime.Now.Year;
            yearMonths.EndMonth = DateTime.Now.Month;
            yearMonths.StartYear = DateTime.Now.Year;
            yearMonths.StartMonth = DateTime.Now.Month;

            for (int i = 0; i < 12; i++)
            {
                yearMonths.EndMonths.Add(new Month { No = i + 1 });
                yearMonths.StartMonths.Add(new Month { No = i + 1 });
            }

            for (int i = -1; i < 10; i++)
            {
                yearMonths.EndYears.Add(new Year { No = DateTime.Now.Year + i });
                yearMonths.StartYears.Add(new Year { No = DateTime.Now.Year + i });
            }

            return yearMonths;
        }
    }

    public class DepreciationPlan
    {
        public DepreciationPlan()
        {
            CurrentCharge = 0;
            CumulativelyCharge = 0;
            RemainingAmount = 0;
            Depreciacions = new List<DepreciationItem>();
        }

        public int No { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthYear { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal CurrentCharge { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal CumulativelyCharge { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal RemainingAmount { get; set; }

        public List<DepreciationItem> Depreciacions { get; set; }
    }

    public class DepreciationPlanList
    {
        public DepreciationPlanList()
        {
            TotalCurrentCharge = 0;
            TotalCumulativelyCharge = 0;
            TotalRemainingAmount = 0;
            DepreciationPlans = new List<DepreciationPlan>();
        }

        public int StartMonth { get; set; }
        public int StartYear { get; set; }
        public int EndMonth { get; set; }
        public int EndYear { get; set; }

        public List<DepreciationPlan> DepreciationPlans { get; set; }

        [Display(Name = "Okres amortyzacji")]
        public string MonthYear { get; set; }
        [Display(Name = "Watrość amortyzacji")]
        public decimal CurrentCharge { get; set; }

        [Display(Name = "Podsumowanie")]
        public string Total { get; set; }
        [Display(Name = "Watrość amortyzacji")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal TotalCurrentCharge { get; set; }
        [Display(Name = "Amortyzacja narastająco")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal TotalCumulativelyCharge { get; set; }
        [Display(Name = "Pozostało do umorzenia")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal TotalRemainingAmount { get; set; }
    }

    public class DepreciationItem
    {
        public DepreciationItem()
        {
            CurrentCharge = 0;
            CumulativelyCharge = 0;
            RemainingAmount = 0;
        }

        [Display(Name = "Środek trwały")]
        public string AssetName { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal CurrentCharge { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal CumulativelyCharge { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal RemainingAmount { get; set; }
    }

    public class AssetDepreciationPlan
    {
        public AssetDepreciationPlan()
        {
            TotalCurrentCharge = 0;
            TotalCumulativelyCharge = 0;
            TotalRemainingAmount = 0;

            AssetDepreciationYearPlans = new List<AssetDepreciationYearPlan>();
        }

        [Display(Name = "Nazwa środka trwałego")]
        public string AssetName { get; set; }
        [Display(Name = "Wartość początkowa środka trwałego")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal InitialValue { get; set; }
        [Display(Name = "Miesiąc rozpoczęcia amortyzacji")]
        public string StartMonth { get; set; }
        [Display(Name = "Stawka amortyzacyjna")]
        public string DepreciationRate { get; set; }
        [Display(Name = "Roczny odpis amortyzacyjny")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal YearCharge { get; set; }
        [Display(Name = "Miesięczny odpis amortyzacyjny")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal MonthlyCharge { get; set; }

        [Display(Name = "Podsumowanie")]
        public string Total { get; set; }
        [Display(Name = "Watrość amortyzacji")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal TotalCurrentCharge { get; set; }
        [Display(Name = "Amortyzacja narastająco")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal TotalCumulativelyCharge { get; set; }
        [Display(Name = "Pozostało do umorzenia")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal TotalRemainingAmount { get; set; }

        public List<AssetDepreciationYearPlan> AssetDepreciationYearPlans { get; set; }
    }

    public class AssetDepreciationYearPlan
    {
        public AssetDepreciationYearPlan()
        {
            AssetDepreciationMonthPlans = new List<AssetDepreciationMonthPlan>();
        }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal TotalYearCharge { get; set; }
        public int Year { get; set; }

        public List<AssetDepreciationMonthPlan> AssetDepreciationMonthPlans { get; set; }
    }

    public class AssetDepreciationMonthPlan
    {
        public int No { get; set; }
        [Display(Name = "miesiąc/rok")]
        public string MonthYear { get; set; }
        [Display(Name = "Kwota odpisów")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal CurrentCharge { get; set; }
        [Display(Name = "Odpisy narastająco")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal CumulativelyCharge { get; set; }
        [Display(Name = "Kwota pozostała do zamortyzowania")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal RemainingAmount { get; set; }
    }

    public class EquipmentElement
    {
        public EquipmentElement()
        {
        }

        public int Id { get; set; }
        [Display(Name = "Nazwa wyposarzenia")]
        public string EquipmentName { get; set; }
        [Display(Name = "Ilosć")]
        public int Quantity { get; set; }
        [Display(Name = "Wartość wg. zakupu")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal InitialValue { get; set; }
        [Display(Name = "Wartość umorzenia")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal AmortisedValue { get; set; }
        [Display(Name = "Pozostało do umorzenia")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal RemainingAmount { get; set; }
    }

    public class EndOfLifeDosposalItem
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
    }

    public class ChangeInValueItem
    {
        public int Id { get; set; }
    }

}