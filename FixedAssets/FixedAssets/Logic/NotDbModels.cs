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
            Months = new List<Logic.Month>();
            Years = new List<Logic.Year>();
        }

        [Display(Name = "Miesiąc zakończenia planu")]
        public int Month { get; set; }
        [Display(Name = "Rok zakończenia planu")]
        public int Year { get; set; }
        public List<Month> Months { get; set; }
        public List<Year> Years { get; set; }
    }

    public class PrepareYearMonths
    {
        public YearMonths GetYearMonths()
        {
            YearMonths yearMonths = new YearMonths();

            yearMonths.Year = DateTime.Now.Year;
            yearMonths.Month = DateTime.Now.Month;

            for (int i = 0; i < 12; i++)
            {
                yearMonths.Months.Add(new Month { No = i + 1 });
            }

            for (int i = 0; i < 10; i++)
            {
                yearMonths.Years.Add(new Year { No = DateTime.Now.Year + i });
            }

            return yearMonths;
        }
    }

    public class DepretiationPlan
    {
        public int No { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthYear { get; set; }
        public decimal CurrentCharge { get; set; }
        public decimal CumulativelyCharge { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}