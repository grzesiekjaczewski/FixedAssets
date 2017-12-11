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

        public void CalculatePlanForAssets( DepreciationPlanList depreciationPlanList,
                                            MyDataSet myDataSet,
                                            bool posting)
        {
            foreach (DepreciationPlan depreciationPlan in depreciationPlanList.DepreciationPlans)
            {
                foreach (Asset asset in myDataSet.AssetList)
                {
                    if (DataManipulation.CanProcessDepretiatin(asset.StartUsingDate, new DateTime(depreciationPlan.Year, depreciationPlan.Month, 1)) 
                        && asset.AmortisedValue < asset.InitialValue)
                    {
                        DepreciationType depreciationType = myDataSet.DepreciationTypes[asset.DepreciationTypeId];
                        decimal depreciation = decimal.Round(asset.InitialValue * ((depreciationType.DepreciationRate / 100) / 12),2);

                        if (!myDataSet.DepreciationCharges.ContainsKey(depreciationPlan.Year.ToString() + depreciationPlan.Month.ToString("00") + asset.Id.ToString()))
                        {
                            if (asset.AmortisedValue + depreciation > asset.InitialValue)
                            {
                                depreciation = asset.InitialValue - asset.AmortisedValue;
                            }
                            asset.AmortisedValue += depreciation;
                            //Księgowanie umożenia
                            if (posting)
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
                        depreciationPlan.Depreciacions.Add(depr);
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
                        depreciationPlanList.TotalCumulativelyCharge += depreciationCharge.CumulativelyCharge;
                        depreciationPlanList.TotalRemainingAmount += depreciationCharge.RemainingAmount;
                    }
                }
            }
        }


        public DepreciationPlanList CalculatePlan(int startMonth, int startYear, int endMonth, int endYear, Dictionary<int, string> monthNames)
        {
            depreciationCalculations depreciationCalculations = new depreciationCalculations();
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



        public void LoadAssets()
        {

        }

    }

    public class depreciationCalculations
    {
        public int CalculateNextdepreciationMonth(DateTime currentDate)
        {
            return currentDate.AddMonths(1).Month;
        }

        public int CalculateNextdepreciationYear(DateTime currentDate)
        {
            return currentDate.AddMonths(1).Year;
        }
    }
}