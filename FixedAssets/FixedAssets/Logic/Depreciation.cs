using FixedAssets.Controllers;
using FixedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixedAssets.Logic
{
    public class Depreciation
    {
        ApplicationDbContext db = new ApplicationDbContext();
        List<DepreciationPlan> _depreciationPlans = new List<DepreciationPlan>();

        public AssetDepreciationPlan CalculateAssetDepreciationPlan(AssetDataSet assetDataSet, Asset asset, bool plan)
        {
            Dictionary<int, int> years = new Dictionary<int, int>();
            int yearIndex = -1;
            AssetDepreciationPlan assetDepreciationPlan = new AssetDepreciationPlan();
            DepreciationCalculations depCalc = new DepreciationCalculations();
            int fistDepMonth = depCalc.CalculateNextDepreciationMonth(asset.StartUsingDate);
            int fistDepYear = depCalc.CalculateNextDepreciationYear(asset.StartUsingDate);
            int fistMonth = asset.StartUsingDate.Month;
            int fistYear = asset.StartUsingDate.Year;
            int no = 0;

            assetDepreciationPlan.StartMonth = assetDataSet.MonthNames[fistDepMonth] + " " + fistDepYear.ToString();
            assetDepreciationPlan.TotalRemainingAmount = asset.InitialValue;
            assetDepreciationPlan.AssetName = asset.AssetName;
            assetDepreciationPlan.DepreciationRate = assetDataSet.DepreciationTypes[asset.DepreciationTypeId].Name;
            assetDepreciationPlan.InitialValue = asset.InitialValue;
            assetDepreciationPlan.YearCharge = decimal.Round((asset.InitialValue * (assetDataSet.DepreciationTypes[asset.DepreciationTypeId].DepreciationRate / 100)) / 12, 2) * (decimal)12;
            assetDepreciationPlan.MonthlyCharge = decimal.Round((asset.InitialValue * (assetDataSet.DepreciationTypes[asset.DepreciationTypeId].DepreciationRate / 100))/12, 2);

            foreach (DepreciationCharge depreciationCharge in assetDataSet.DepreciationCharges)
            {
                if (!years.ContainsKey(depreciationCharge.Year))
                {
                    yearIndex++;
                    years.Add(depreciationCharge.Year, depreciationCharge.Year);
                    AssetDepreciationYearPlan assetDepreciationYearPlan = new AssetDepreciationYearPlan();
                    AssetDepreciationMonthPlan assetDepreciationMonthPlan = new AssetDepreciationMonthPlan();

                    assetDepreciationMonthPlan.No = depreciationCharge.No;
                    assetDepreciationMonthPlan.MonthYear = assetDataSet.MonthNames[depreciationCharge.Month] + " " + depreciationCharge.Year.ToString();
                    assetDepreciationMonthPlan.CurrentCharge = depreciationCharge.CurrentCharge;
                    assetDepreciationMonthPlan.CumulativelyCharge = depreciationCharge.CumulativelyCharge;
                    assetDepreciationMonthPlan.RemainingAmount = depreciationCharge.RemainingAmount;

                    assetDepreciationYearPlan.TotalYearCharge += depreciationCharge.CurrentCharge;
                    assetDepreciationYearPlan.Year = depreciationCharge.Year;
                    assetDepreciationYearPlan.AssetDepreciationMonthPlans.Add(assetDepreciationMonthPlan);

                    assetDepreciationPlan.AssetDepreciationYearPlans.Add(assetDepreciationYearPlan);
                }
                else
                {
                    AssetDepreciationYearPlan assetDepreciationYearPlan = assetDepreciationPlan.AssetDepreciationYearPlans[yearIndex];
                    AssetDepreciationMonthPlan assetDepreciationMonthPlan = new AssetDepreciationMonthPlan();

                    assetDepreciationMonthPlan.No = depreciationCharge.No;
                    assetDepreciationMonthPlan.MonthYear = assetDataSet.MonthNames[depreciationCharge.Month] + " " + depreciationCharge.Year.ToString();
                    assetDepreciationMonthPlan.CurrentCharge = depreciationCharge.CurrentCharge;
                    assetDepreciationMonthPlan.CumulativelyCharge = depreciationCharge.CumulativelyCharge;
                    assetDepreciationMonthPlan.RemainingAmount = depreciationCharge.RemainingAmount;

                    assetDepreciationYearPlan.TotalYearCharge += depreciationCharge.CurrentCharge;

                    assetDepreciationYearPlan.AssetDepreciationMonthPlans.Add(assetDepreciationMonthPlan);
                }

                assetDepreciationPlan.TotalCurrentCharge += depreciationCharge.CurrentCharge;
                assetDepreciationPlan.TotalCumulativelyCharge += depreciationCharge.CurrentCharge;
                assetDepreciationPlan.TotalRemainingAmount -= depreciationCharge.CurrentCharge;

                fistMonth = depreciationCharge.Month;
                fistYear = depreciationCharge.Year;
                no = depreciationCharge.No;
            }

            if (!plan || assetDepreciationPlan.TotalCumulativelyCharge == asset.InitialValue) return assetDepreciationPlan;


            bool next = true;
            while(next)
            {
                fistMonth++;
                no++;
                if(fistMonth > 12)
                {
                    fistMonth = 1;
                    fistYear++;
                }
                decimal depreciation = assetDepreciationPlan.MonthlyCharge;
                if (assetDepreciationPlan.TotalCumulativelyCharge + depreciation >= asset.InitialValue)
                {
                    next = false;
                    depreciation = asset.InitialValue - assetDepreciationPlan.TotalCumulativelyCharge;
                }

                assetDepreciationPlan.TotalCurrentCharge += depreciation;
                assetDepreciationPlan.TotalCumulativelyCharge += depreciation;
                assetDepreciationPlan.TotalRemainingAmount -= depreciation;

                if (!years.ContainsKey(fistYear))
                {
                    yearIndex++;
                    years.Add(fistYear, fistYear);
                    AssetDepreciationYearPlan assetDepreciationYearPlan = new AssetDepreciationYearPlan();
                    AssetDepreciationMonthPlan assetDepreciationMonthPlan = new AssetDepreciationMonthPlan();

                    assetDepreciationMonthPlan.No = no;
                    assetDepreciationMonthPlan.MonthYear = assetDataSet.MonthNames[fistMonth] + " " + fistYear.ToString();
                    assetDepreciationMonthPlan.CurrentCharge = depreciation;
                    assetDepreciationMonthPlan.CumulativelyCharge = assetDepreciationPlan.TotalCumulativelyCharge;
                    assetDepreciationMonthPlan.RemainingAmount = assetDepreciationPlan.TotalRemainingAmount;

                    assetDepreciationYearPlan.TotalYearCharge += depreciation;
                    assetDepreciationYearPlan.Year = fistYear;
                    assetDepreciationYearPlan.AssetDepreciationMonthPlans.Add(assetDepreciationMonthPlan);

                    assetDepreciationPlan.AssetDepreciationYearPlans.Add(assetDepreciationYearPlan);
                }
                else
                {
                    AssetDepreciationYearPlan assetDepreciationYearPlan = assetDepreciationPlan.AssetDepreciationYearPlans[yearIndex];
                    AssetDepreciationMonthPlan assetDepreciationMonthPlan = new AssetDepreciationMonthPlan();

                    assetDepreciationMonthPlan.No = no;
                    assetDepreciationMonthPlan.MonthYear = assetDataSet.MonthNames[fistMonth] + " " + fistYear.ToString();
                    assetDepreciationMonthPlan.CurrentCharge = depreciation;
                    assetDepreciationMonthPlan.CumulativelyCharge = assetDepreciationPlan.TotalCumulativelyCharge;
                    assetDepreciationMonthPlan.RemainingAmount = assetDepreciationPlan.TotalRemainingAmount;

                    assetDepreciationYearPlan.TotalYearCharge += depreciation;

                    assetDepreciationYearPlan.AssetDepreciationMonthPlans.Add(assetDepreciationMonthPlan);
                }
            }

            return assetDepreciationPlan;
        }


        public void CalculatePlanForAssets( DepreciationPlanList depreciationPlanList,
                                            MyDataSet myDataSet,
                                            bool posting)
        {
            foreach (DepreciationPlan depreciationPlan in depreciationPlanList.DepreciationPlans)
            {
                foreach (Asset asset in myDataSet.AssetList)
                {
                    if (DataManipulation.CanProcessDepretiatin(asset.StartUsingDate, new DateTime(depreciationPlan.Year, depreciationPlan.Month, 1)))
                    {
                        DepreciationType depreciationType = myDataSet.DepreciationTypes[asset.DepreciationTypeId];
                        AssetType assetType = myDataSet.AssetTypes.Where(at => at.Id == asset.AssetTypeId).FirstOrDefault();
                        decimal depreciation = 0;
                        if (assetType.LowValueAsset)
                        {
                            depreciation = asset.InitialValue;
                        }
                        else
                        {
                            depreciation = decimal.Round(asset.InitialValue * ((depreciationType.DepreciationRate / 100) / 12), 2);
                        }

                        if (!myDataSet.DepreciationCharges.ContainsKey(depreciationPlan.Year.ToString() + depreciationPlan.Month.ToString("00") + asset.Id.ToString()) && depreciation!=0)
                        {
                            if (asset.AmortisedValue + depreciation > asset.InitialValue)
                            {
                                depreciation = asset.InitialValue - asset.AmortisedValue;
                            }
                            asset.AmortisedValue += depreciation;
                            //Księgowanie umożenia
                            if (posting && asset.AmortisedValue <= asset.InitialValue && asset.IsUsed && !asset.Depreciated && depreciation != 0)
                            {
                                DepreciationCharge depreciationCharge = new DepreciationCharge();
                                depreciationCharge.Month = depreciationPlan.Month;
                                depreciationCharge.Year = depreciationPlan.Year;
                                depreciationCharge.AssetId = asset.Id;
                                depreciationCharge.CurrentCharge = depreciation;
                                depreciationCharge.RemainingAmount = asset.InitialValue - asset.AmortisedValue;
                                depreciationCharge.CumulativelyCharge = asset.AmortisedValue;
                                depreciationCharge.No = db.T_DepreciationCharges.Where(dc => dc.AssetId == asset.Id).ToList().Count + 1;
                                db.T_DepreciationCharges.Add(depreciationCharge);

                                Asset assetForModfy = db.T_Assets.Find(asset.Id);
                                assetForModfy.AmortisedValue += depreciation;
                                if (assetForModfy.AmortisedValue == assetForModfy.InitialValue)
                                {
                                    assetForModfy.Depreciated = true;
                                }

                                db.SaveChanges();
                            }
                        }
                        DepreciationItem depr = new DepreciationItem();
                        depr.AssetName = asset.AssetName;
                        depr.CurrentCharge = depreciation;
                        if (depreciation != 0)
                        {
                            depreciationPlan.Depreciacions.Add(depr);
                        }
                        depreciationPlan.CurrentCharge += depreciation;
                        depreciationPlanList.TotalCurrentCharge += depreciation;
                    }
                }
            }
        }

        public void CalculateProcessedDepreciation(DepreciationPlanList depreciationPlanList,
                                    MyDataSet myDataSet)
        {
            foreach (DepreciationPlan depreciationPlan in depreciationPlanList.DepreciationPlans)
            {
                foreach (Asset asset in myDataSet.AssetList)
                {
                    DepreciationType depreciationType = myDataSet.DepreciationTypes[asset.DepreciationTypeId];
                    if (myDataSet.DepreciationCharges.ContainsKey(depreciationPlan.Year.ToString() + depreciationPlan.Month.ToString("00") + asset.Id.ToString()))
                    {
                        DepreciationCharge depreciationCharge = myDataSet.DepreciationCharges[depreciationPlan.Year.ToString() + depreciationPlan.Month.ToString("00") + asset.Id.ToString()];

                        DepreciationItem depr = new DepreciationItem();
                        depr.AssetName = asset.AssetName;
                        depr.CurrentCharge = depreciationCharge.CurrentCharge;
                        depr.CumulativelyCharge = depreciationCharge.CumulativelyCharge;
                        depr.RemainingAmount = depreciationCharge.RemainingAmount;
                        depreciationPlan.Depreciacions.Add(depr);

                        depreciationPlan.CurrentCharge += depreciationCharge.CurrentCharge;
                        depreciationPlan.CumulativelyCharge += depreciationCharge.CumulativelyCharge;
                        depreciationPlan.RemainingAmount += depreciationCharge.RemainingAmount;

                        depreciationPlanList.TotalCurrentCharge += depreciationCharge.CurrentCharge;
                    }
                }
                if (depreciationPlan.CumulativelyCharge != 0)
                {
                    depreciationPlanList.TotalCumulativelyCharge = depreciationPlan.CumulativelyCharge;
                    depreciationPlanList.TotalRemainingAmount = depreciationPlan.RemainingAmount;
                }
            }
        }

        public DepreciationPlanList CalculatePlan(int startMonth, int startYear, int endMonth, int endYear, Dictionary<int, string> monthNames)
        {
            DepreciationCalculations depreciationCalculations = new DepreciationCalculations();
            DepreciationPlanList depreciationPlanList = new DepreciationPlanList();

            depreciationPlanList.StartMonth = startMonth;
            depreciationPlanList.StartYear = startYear;
            depreciationPlanList.EndMonth = endMonth;
            depreciationPlanList.EndYear = endYear;

            int no = 1;
            for (int y = startYear; y < endYear + 1; y++)
            {
                if (startYear == endYear)
                {
                    for (int m = startMonth; m <= endMonth; m++)
                    {
                        DepreciationPlan depreciationPlan = preparedepreciationPeriod(no, y, m, monthNames);
                        depreciationPlanList.DepreciationPlans.Add(depreciationPlan);
                        no++;
                    }
                }
                else
                {
                    if (startYear == y)
                    {
                        for (int m = startMonth; m <= 12; m++)
                        {
                            DepreciationPlan depreciationPlan = preparedepreciationPeriod(no, y, m, monthNames);
                            depreciationPlanList.DepreciationPlans.Add(depreciationPlan);
                            no++;
                        }
                    }
                    if (endYear == y)
                    {
                        for (int m = 1; m <= endMonth; m++)
                        {
                            DepreciationPlan depreciationPlan = preparedepreciationPeriod(no, y, m, monthNames);
                            depreciationPlanList.DepreciationPlans.Add(depreciationPlan);
                            no++;
                        }
                    }
                    if (startYear < y && y < endYear)
                    {
                        for (int m = 1; m <= 12; m++)
                        {
                            DepreciationPlan depreciationPlan = preparedepreciationPeriod(no, y, m, monthNames);
                            depreciationPlanList.DepreciationPlans.Add(depreciationPlan);
                            no++;
                        }
                    }
                }
            }
            return depreciationPlanList;
        }

        private DepreciationPlan preparedepreciationPeriod(int no, int y, int m, Dictionary<int, string> monthNames)
        {
            DepreciationPlan depreciationPlan = new DepreciationPlan();
            depreciationPlan.No = no;
            depreciationPlan.Year = y;
            depreciationPlan.Month = m;
            depreciationPlan.MonthYear = monthNames[m] + " " + y.ToString();

            return depreciationPlan;
        }
    }

    public class DepreciationCalculations
    {
        public int CalculateNextDepreciationMonth(DateTime currentDate)
        {
            return currentDate.AddMonths(1).Month;
        }

        public int CalculateNextDepreciationYear(DateTime currentDate)
        {
            return currentDate.AddMonths(1).Year;
        }
    }
}