using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FixedAssets.Logic;
using System.Collections.Generic;
using FixedAssets;
using FixedAssets.Models;
using FixedAssets.Controllers;

namespace UnitTestForFixesAssets
{

    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethodCanProcessDepretiatin()
        {
            Assert.IsFalse(DataManipulation.CanProcessDepretiatin(new DateTime(2017, 1, 1), new DateTime(2017, 1, 1)));
            Assert.IsFalse(DataManipulation.CanProcessDepretiatin(new DateTime(2017, 2, 1), new DateTime(2017, 1, 1)));
            Assert.IsTrue(DataManipulation.CanProcessDepretiatin(new DateTime(2016, 2, 1), new DateTime(2017, 3, 1)));
            Assert.IsFalse(DataManipulation.CanProcessDepretiatin(new DateTime(2017, 8, 1), new DateTime(2016, 3, 1)));
            Assert.IsTrue(DataManipulation.CanProcessDepretiatin(new DateTime(2017, 8, 1), new DateTime(2017, 9, 1)));
        }

        [TestMethod]
        public void TestMethodStartDepretiatinPeriod()
        {
            depreciationCalculations depreciationCalculations = new depreciationCalculations();
            DateTime currentDate;
            int startMonth;
            int startYear;
            for (int y = 2016; y < 2020; y++)
            {
                for (int m = 1; m < 12; m++)
                {
                    currentDate = new DateTime(y, m, 1);
                    startMonth = depreciationCalculations.CalculateNextdepreciationMonth(currentDate);
                    Assert.AreEqual(startMonth, m + 1);
                    startYear = depreciationCalculations.CalculateNextdepreciationYear(currentDate);
                    Assert.AreEqual(startYear, y);
                }
                currentDate = new DateTime(y, 12, 1);
                startMonth = depreciationCalculations.CalculateNextdepreciationMonth(currentDate);
                Assert.AreEqual(startMonth, 1);
                startYear = depreciationCalculations.CalculateNextdepreciationYear(currentDate);
                Assert.AreEqual(startYear, y + 1);
            }
        }

        public Dictionary<int, string> MocForMonths()
        {
            Dictionary<int, string> monthNames = new Dictionary<int, string>();
            monthNames.Add(1, "styczeń");
            monthNames.Add(2, "luty");
            monthNames.Add(3, "marzec");
            monthNames.Add(4, "kwiecień");
            monthNames.Add(5, "maj");
            monthNames.Add(6, "czerwiec");
            monthNames.Add(7, "lipiec");
            monthNames.Add(8, "sierpień");
            monthNames.Add(9, "wrzesień");
            monthNames.Add(10, "październik");
            monthNames.Add(11, "listopad");
            monthNames.Add(12, "grudzień");

            return monthNames;
        }

        [TestMethod]
        public void TestMethodDepretiatinPlan()
        {
            Depreciation depreciation = new Depreciation();
            Dictionary<int, string> monthNames = MocForMonths();

            int startMonth = 2, startYear = 2017, endMonth = 5, endYear = 2018;

            DepreciationPlanList depreciationPlanList = depreciation.CalculatePlan(startMonth, startYear, endMonth, endYear, monthNames);

            Assert.IsTrue(depreciationPlanList.DepreciationPlans.Count == 16);
        }

        [TestMethod]
        public void TestMethodDepretiatinPlanCalculation()
        {
            Depreciation depreciation = new Depreciation();
            Dictionary<int, string> monthNames = MocForMonths();

            int startMonth = 2, startYear = 2017, endMonth = 5, endYear = 2030;

            DepreciationPlanList depreciationPlanList = depreciation.CalculatePlan(startMonth, startYear, endMonth, endYear, monthNames);

            List<Asset> assetList = new List<Asset>();

            assetList.Add(new Asset() { Id = 2 , AssetName = "Serwer HP", StartUsingDate = new DateTime(2017,12,8), InitialValue = (decimal)1000.21, AmortisedValue = 0, DepreciationTypeId = 1 });
            assetList.Add(new Asset() { Id = 3, AssetName = "Macierz dyskowa 50TB", StartUsingDate = new DateTime(2017, 12, 21), InitialValue = (decimal)25000, AmortisedValue = 0, DepreciationTypeId = 1 });
            assetList.Add(new Asset() { Id = 6, AssetName = "Biurko prezesowej", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)10000.00, AmortisedValue = 0, DepreciationTypeId = 1 });

            Dictionary<int, DepreciationType> depreciationTypes = new Dictionary<int, DepreciationType>();
            depreciationTypes.Add(1, new DepreciationType() { Id = 1, Name = "Liniowa 30%", DepreciationRate = (decimal)30 });

            Dictionary<string, DepreciationCharge> depreciationCharges = new Dictionary<string, DepreciationCharge>();
            MyDataSet myDataSet = new MyDataSet();
            myDataSet.AssetList = assetList;
            myDataSet.DepreciationTypes = depreciationTypes;
            myDataSet.DepreciationCharges = depreciationCharges;

            depreciation.CalculatePlanForAssets(depreciationPlanList, myDataSet, false);

            decimal total = 0;
            foreach(DepreciationPlan dep in depreciationPlanList.DepreciationPlans)
            {
                total += dep.CurrentCharge;
            }

            Assert.AreEqual(total, (decimal)(10000.00 + 25000.00 + 1000.21));
        }

        [TestMethod]
        public void TestMethodDepretiatinProccessedView()
        {
            Depreciation depreciation = new Depreciation();
            Dictionary<int, string> monthNames = MocForMonths();

            int startMonth = 1, startYear = 2018, endMonth = 2, endYear = 2018;

            DepreciationPlanList depreciationPlanList = depreciation.CalculatePlan(startMonth, startYear, endMonth, endYear, monthNames);

            List<Asset> assetList = new List<Asset>();

            assetList.Add(new Asset() { Id = 2, AssetName = "Serwer HP", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)1000.21, AmortisedValue = 0, DepreciationTypeId = 1 });
            assetList.Add(new Asset() { Id = 3, AssetName = "Macierz dyskowa 50TB", StartUsingDate = new DateTime(2017, 12, 21), InitialValue = (decimal)25000, AmortisedValue = 0, DepreciationTypeId = 1 });
            assetList.Add(new Asset() { Id = 6, AssetName = "Biurko prezesowej", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)10000.00, AmortisedValue = 0, DepreciationTypeId = 1 });

            Dictionary<int, DepreciationType> depreciationTypes = new Dictionary<int, DepreciationType>();
            depreciationTypes.Add(1, new DepreciationType() { Id = 1, Name = "Liniowa 30%", DepreciationRate = (decimal)30 });

            Dictionary<string, DepreciationCharge> depreciationCharges = new Dictionary<string, DepreciationCharge>();
            depreciationCharges.Add("2018012", new DepreciationCharge() { Id = 10, No = 1, Month = 1, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)25.01, RemainingAmount = (decimal)975.20, AssetId = 2 });
            depreciationCharges.Add("2018013", new DepreciationCharge() { Id = 11, No = 1, Month = 1, Year = 2018, CurrentCharge = (decimal)625.00, CumulativelyCharge = (decimal)25.01, RemainingAmount = (decimal)24375.00, AssetId = 3 });
            depreciationCharges.Add("2018016", new DepreciationCharge() { Id = 12, No = 1, Month = 1, Year = 2018, CurrentCharge = (decimal)250.00, CumulativelyCharge = (decimal)25.01, RemainingAmount = (decimal)9750.00, AssetId = 6 });
            depreciationCharges.Add("2018022", new DepreciationCharge() { Id = 13, No = 2, Month = 2, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)50.02, RemainingAmount = (decimal)950.19, AssetId = 2 });
            depreciationCharges.Add("2018023", new DepreciationCharge() { Id = 14, No = 2, Month = 2, Year = 2018, CurrentCharge = (decimal)625.00, CumulativelyCharge = (decimal)1250.00, RemainingAmount = (decimal)23750.00, AssetId = 3 });
            depreciationCharges.Add("2018026", new DepreciationCharge() { Id = 15, No = 2, Month = 2, Year = 2018, CurrentCharge = (decimal)250.00, CumulativelyCharge = (decimal)500.00, RemainingAmount = (decimal)9500.00, AssetId = 6 });

            MyDataSet myDataSet = new MyDataSet();
            myDataSet.AssetList = assetList;
            myDataSet.DepreciationTypes = depreciationTypes;
            myDataSet.DepreciationCharges = depreciationCharges;

            depreciation.CalculateProcessedDepreciation(depreciationPlanList, myDataSet);

            decimal total11 = 0;
            decimal total12 = 0;
            decimal total13 = 0;

            decimal total21 = 0;
            decimal total22 = 0;
            decimal total23 = 0;

            decimal total31 = 0;
            decimal total32 = 0;
            decimal total33 = 0;
            foreach (DepreciationPlan dep in depreciationPlanList.DepreciationPlans)
            {
                foreach (DepreciationItem d in dep.Depreciacions)
                {
                    total11 += d.CurrentCharge;
                    total12 += d.CumulativelyCharge;
                    total13 += d.RemainingAmount;
                }

                total21 += dep.CurrentCharge;
                total22 += dep.CumulativelyCharge;
                total23 += dep.RemainingAmount;
            }

            total31 += depreciationPlanList.TotalCurrentCharge;
            total32 += depreciationPlanList.TotalCumulativelyCharge;
            total33 += depreciationPlanList.TotalRemainingAmount;


            Assert.AreEqual(total11 + total12 + total13, total21 + total22 + total23);
            Assert.AreEqual(total31 + total32 + total33, total21 + total22 + total23);
        }

    }
}
