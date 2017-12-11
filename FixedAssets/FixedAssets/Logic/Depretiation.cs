using FixedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixedAssets.Logic
{
    public class Depretiation
    {
        ApplicationDbContext db = new ApplicationDbContext();
        List<DepretiationPlan> _depretiationPlans = new List<DepretiationPlan>();

        public void CalculatePlanForAssets( DepretiationPlanList depretiationPlanList,
                                            List<Asset> assetList,
                                            Dictionary<int, DepreciationType> depreciationTypes,
                                            Dictionary<string, DepreciationCharge> depreciationCharges)
        {
            foreach (DepretiationPlan depretiationPlan in depretiationPlanList.DepretiationPlans)
            {
                foreach (Asset asset in assetList)
                {
                    if (DataManipulation.CanProcessDepretiatin(asset.StartUsingDate, new DateTime(depretiationPlan.Year, depretiationPlan.Month, 1)) 
                        && asset.AmortisedValue < asset.InitialValue)
                    {
                        DepreciationType depreciationType = depreciationTypes[asset.DepreciationTypeId];
                        decimal depretiation = decimal.Round(asset.InitialValue * ((depreciationType.DepreciationRate / 100) / 12),2);

                        if (!depreciationCharges.ContainsKey(depretiationPlan.Year.ToString() + depretiationPlan.Month.ToString("00") + asset.Id.ToString()))
                        {
                            if (asset.AmortisedValue + depretiation > asset.InitialValue)
                            {
                                depretiation = asset.InitialValue - asset.AmortisedValue;
                            }
                            asset.AmortisedValue += depretiation;
                        }

                        depretiationPlan.CurrentCharge += depretiation;
                        depretiationPlanList.TotalCurrentCharge += depretiation;
                    }
                }
            }
        }

        public void CalculateDepretiationForAssets(DepretiationPlanList depretiationPlanList,
                                            List<Asset> assetList,
                                            Dictionary<int, DepreciationType> depreciationTypes,
                                            Dictionary<string, DepreciationCharge> depreciationCharges)
        {
            foreach (DepretiationPlan depretiationPlan in depretiationPlanList.DepretiationPlans)
            {
                foreach (Asset asset in assetList)
                {
                    if (DataManipulation.CanProcessDepretiatin(asset.StartUsingDate, new DateTime(depretiationPlan.Year, depretiationPlan.Month, 1))
                        && asset.AmortisedValue < asset.InitialValue)
                    {
                        DepreciationType depreciationType = depreciationTypes[asset.DepreciationTypeId];
                        decimal depretiation = decimal.Round(asset.InitialValue * ((depreciationType.DepreciationRate / 100) / 12), 2);
                        if (!depreciationCharges.ContainsKey(depretiationPlan.Year.ToString() + depretiationPlan.Month.ToString("00") + asset.Id.ToString()))
                        {
                            if (asset.AmortisedValue + depretiation > asset.InitialValue)
                            {
                                depretiation = asset.InitialValue - asset.AmortisedValue;
                            }
                            asset.AmortisedValue += depretiation;
                            //Księgowanie umożenia
                            {
                                DepreciationCharge depreciationCharge = new DepreciationCharge();
                                depreciationCharge.Month = depretiationPlan.Month;
                                depreciationCharge.Year = depretiationPlan.Year;
                                depreciationCharge.Asset_Id = asset.Id;
                                depreciationCharge.CurrentCharge = depretiation;
                                depreciationCharge.RemainingAmount = asset.AmortisedValue - asset.AmortisedValue;
                                depreciationCharge.CumulativelyCharge = asset.AmortisedValue;
                                depreciationCharge.No = db.T_DepreciationCharges.Where(dc => dc.Asset_Id == asset.Id).ToList().Count + 1;
                                db.T_DepreciationCharges.Add(depreciationCharge);

                                Asset assetForModfy = db.T_Assets.Find(asset.Id);
                                assetForModfy.AmortisedValue += depretiation;
                                if (assetForModfy.AmortisedValue == assetForModfy.InitialValue)
                                {
                                    assetForModfy.Depreciated = true;
                                }

                                db.SaveChanges();
                            }
                        }
                        
                        depretiationPlan.CurrentCharge += depretiation;
                    }
                }
            }
        }

        public DepretiationPlanList CalculatePlan(int startMonth, int startYear, int endMonth, int endYear, Dictionary<int, string> monthNames)
        {
            DepretiationCalculations depretiationCalculations = new DepretiationCalculations();
            DepretiationPlanList depretiationPlanList = new DepretiationPlanList();

            depretiationPlanList.StartMonth = startMonth;
            depretiationPlanList.StartYear = startYear;
            depretiationPlanList.EndMonth = endMonth;
            depretiationPlanList.EndYear = endYear;

            int no = 1;
            for (int y = startYear; y < endYear + 1; y++)
            {
                if (startYear == endYear)
                {
                    for (int m = startMonth; m <= endMonth; m++)
                    {
                        DepretiationPlan depretiationPlan = prepareDepretiationPeriod(no, y, m, monthNames);
                        depretiationPlanList.DepretiationPlans.Add(depretiationPlan);
                        no++;
                    }
                }
                else
                {
                    if (startYear == y)
                    {
                        for (int m = startMonth; m <= 12; m++)
                        {
                            DepretiationPlan depretiationPlan = prepareDepretiationPeriod(no, y, m, monthNames);
                            depretiationPlanList.DepretiationPlans.Add(depretiationPlan);
                            no++;
                        }
                    }
                    if (endYear == y)
                    {
                        for (int m = 1; m <= endMonth; m++)
                        {
                            DepretiationPlan depretiationPlan = prepareDepretiationPeriod(no, y, m, monthNames);
                            depretiationPlanList.DepretiationPlans.Add(depretiationPlan);
                            no++;
                        }
                    }
                    if (startYear < y && y < endYear)
                    {
                        for (int m = 1; m <= 12; m++)
                        {
                            DepretiationPlan depretiationPlan = prepareDepretiationPeriod(no, y, m, monthNames);
                            depretiationPlanList.DepretiationPlans.Add(depretiationPlan);
                            no++;
                        }
                    }
                }
            }
            return depretiationPlanList;
        }

        private DepretiationPlan prepareDepretiationPeriod(int no, int y, int m, Dictionary<int, string> monthNames)
        {
            DepretiationPlan depretiationPlan = new DepretiationPlan();
            depretiationPlan.No = no;
            depretiationPlan.Year = y;
            depretiationPlan.Month = m;
            depretiationPlan.MonthYear = monthNames[m] + " " + y.ToString();

            return depretiationPlan;
        }



        public void LoadAssets()
        {

        }

    }

    public class DepretiationCalculations
    {
        public int CalculateNextDepretiationMonth(DateTime currentDate)
        {
            return currentDate.AddMonths(1).Month;
        }

        public int CalculateNextDepretiationYear(DateTime currentDate)
        {
            return currentDate.AddMonths(1).Year;
        }
    }
}