using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FixedAssets.Logic;
using System.Collections.Generic;
using FixedAssets;
using FixedAssets.Models;

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
            DepretiationCalculations depretiationCalculations = new DepretiationCalculations();
            DateTime currentDate;
            int startMonth;
            int startYear;
            for (int y = 2016; y < 2020; y++)
            {
                for (int m = 1; m < 12; m++)
                {
                    currentDate = new DateTime(y, m, 1);
                    startMonth = depretiationCalculations.CalculateNextDepretiationMonth(currentDate);
                    Assert.AreEqual(startMonth, m + 1);
                    startYear = depretiationCalculations.CalculateNextDepretiationYear(currentDate);
                    Assert.AreEqual(startYear, y);
                }
                currentDate = new DateTime(y, 12, 1);
                startMonth = depretiationCalculations.CalculateNextDepretiationMonth(currentDate);
                Assert.AreEqual(startMonth, 1);
                startYear = depretiationCalculations.CalculateNextDepretiationYear(currentDate);
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
            Depretiation depretiation = new Depretiation();
            Dictionary<int, string> monthNames = MocForMonths();

            int startMonth = 2, startYear = 2017, endMonth = 5, endYear = 2018;

            DepretiationPlanList depretiationPlanList = depretiation.CalculatePlan(startMonth, startYear, endMonth, endYear, monthNames);

            Assert.IsTrue(depretiationPlanList.DepretiationPlans.Count == 16);
        }

        [TestMethod]
        public void TestMethodDepretiatinPlanCalculation()
        {
            Depretiation depretiation = new Depretiation();
            Dictionary<int, string> monthNames = MocForMonths();

            int startMonth = 2, startYear = 2017, endMonth = 5, endYear = 2030;

            DepretiationPlanList depretiationPlanList = depretiation.CalculatePlan(startMonth, startYear, endMonth, endYear, monthNames);

            List<Asset> assetList = new List<Asset>();

            assetList.Add(new Asset() { Id = 2 , AssetName = "Serwer HP", StartUsingDate = new DateTime(2017,12,8), InitialValue = (decimal)1000.21, AmortisedValue = 0, DepreciationTypeId = 1 });
            assetList.Add(new Asset() { Id = 3, AssetName = "Macierz dyskowa 50TB", StartUsingDate = new DateTime(2017, 12, 21), InitialValue = (decimal)25000, AmortisedValue = 0, DepreciationTypeId = 1 });
            assetList.Add(new Asset() { Id = 6, AssetName = "Biurko prezesowej", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)10000.00, AmortisedValue = 0, DepreciationTypeId = 1 });

            Dictionary<int, DepreciationType> depreciationTypes = new Dictionary<int, DepreciationType>();
            depreciationTypes.Add(1, new DepreciationType() { Id = 1, Name = "Liniowa 30%", DepreciationRate = (decimal)30 });

            depretiation.CalculatePlanForAssets(depretiationPlanList, assetList, depreciationTypes);

            decimal total = 0;
            foreach(DepretiationPlan dep in depretiationPlanList.DepretiationPlans)
            {
                total += dep.CurrentCharge;
            }

            Assert.AreEqual(total, (decimal)(10000.00 + 25000.00 + 1000.21));
        }

    }
}
