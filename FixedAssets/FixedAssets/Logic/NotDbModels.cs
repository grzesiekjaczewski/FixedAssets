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

            //if (StartMonth > 1)
            //{
            //    StartMonth--;
            //    EndMonth--;
            //    StartYear--;
            //}
            //else
            //{
            //    StartMonth = 1;
            //    EndMonth = 1;
            //    StartYear-=2;
            //    EndYear--;
            //}
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
            DepreciationPlans = new List<DepreciationPlan>();
            TotalCurrentCharge = 0;
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
        [Display(Name = "Środek trwały")]
        public string AssetName { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal CurrentCharge { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal CumulativelyCharge { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = false)]
        public decimal RemainingAmount { get; set; }
    }
}