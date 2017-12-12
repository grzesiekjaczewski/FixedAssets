using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FixedAssets.Logic;
using System.Collections.Generic;
using FixedAssets;
using FixedAssets.Models;
using FixedAssets.Controllers;
using System.Linq;

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
            DepreciationCalculations depreciationCalculations = new DepreciationCalculations();
            DateTime currentDate;
            int startMonth;
            int startYear;
            for (int y = 2016; y < 2020; y++)
            {
                for (int m = 1; m < 12; m++)
                {
                    currentDate = new DateTime(y, m, 1);
                    startMonth = depreciationCalculations.CalculateNextDepreciationMonth(currentDate);
                    Assert.AreEqual(startMonth, m + 1);
                    startYear = depreciationCalculations.CalculateNextDepreciationYear(currentDate);
                    Assert.AreEqual(startYear, y);
                }
                currentDate = new DateTime(y, 12, 1);
                startMonth = depreciationCalculations.CalculateNextDepreciationMonth(currentDate);
                Assert.AreEqual(startMonth, 1);
                startYear = depreciationCalculations.CalculateNextDepreciationYear(currentDate);
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

            List<AssetType> assetTypes = new List<AssetType>();

            assetTypes.Add(new AssetType() { Id = 3, LowValueAsset = false, Name = "Zestaw komputerowy" });
            assetTypes.Add(new AssetType() { Id = 4, LowValueAsset = false, Name = "Drukarka" });
            assetTypes.Add(new AssetType() { Id = 5, LowValueAsset = false, Name = "Serwer" });
            assetTypes.Add(new AssetType() { Id = 6, LowValueAsset = true, Name = "Biurko" });
            assetTypes.Add(new AssetType() { Id = 7, LowValueAsset = true, Name = "Krzeslo" });
            assetTypes.Add(new AssetType() { Id = 8, LowValueAsset = false, Name = "Projektor" });
            assetTypes.Add(new AssetType() { Id = 9, LowValueAsset = false, Name = "UPS" });
            assetTypes.Add(new AssetType() { Id = 10, LowValueAsset = false, Name = "Macierz dyskowa" });
            assetTypes.Add(new AssetType() { Id = 11, LowValueAsset = false, Name = "Super biurko prezesowej" });
            List<Asset> assetList = new List<Asset>();

            assetList.Add(new Asset() { Id = 2 , AssetName = "Serwer HP", StartUsingDate = new DateTime(2017,12,8), InitialValue = (decimal)1000.21, AmortisedValue = 0, DepreciationTypeId = 1, AssetTypeId = 5, IsUsed = true, Depreciated = false });
            assetList.Add(new Asset() { Id = 3, AssetName = "Macierz dyskowa 50TB", StartUsingDate = new DateTime(2017, 12, 21), InitialValue = (decimal)25000, AmortisedValue = 0, DepreciationTypeId = 1, AssetTypeId = 10, IsUsed = true, Depreciated = false });
            assetList.Add(new Asset() { Id = 6, AssetName = "Biurko prezesowej", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)10000.00, AmortisedValue = 0, DepreciationTypeId = 1, AssetTypeId = 11, IsUsed = true, Depreciated = false });
            assetList.Add(new Asset() { Id = 9, AssetName = "Biurko pracownika", StartUsingDate = new DateTime(2017, 10, 15), InitialValue = (decimal)500.00, AmortisedValue = 0, DepreciationTypeId = 2, AssetTypeId = 6, IsUsed = true, Depreciated = false });

            Dictionary<int, DepreciationType> depreciationTypes = new Dictionary<int, DepreciationType>();
            depreciationTypes.Add(1, new DepreciationType() { Id = 1, Name = "Liniowa 30%", DepreciationRate = (decimal)30 });
            depreciationTypes.Add(2, new DepreciationType() { Id = 2, Name = "Jednorazowa", DepreciationRate = (decimal)100 });

            Dictionary<string, DepreciationCharge> depreciationCharges = new Dictionary<string, DepreciationCharge>();
            MyDataSet myDataSet = new MyDataSet();
            myDataSet.AssetList = assetList;
            myDataSet.DepreciationTypes = depreciationTypes;
            myDataSet.DepreciationCharges = depreciationCharges;
            myDataSet.AssetTypes = assetTypes;

            depreciation.CalculatePlanForAssets(depreciationPlanList, myDataSet, false);

            decimal total = 0;
            foreach(DepreciationPlan dep in depreciationPlanList.DepreciationPlans)
            {
                total += dep.CurrentCharge;
            }

            Assert.AreEqual(total, (decimal)(10000.00 + 25000.00 + 1000.21 + 500));
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
                total22 = dep.CumulativelyCharge;
                total23 = dep.RemainingAmount;
            }

            total31 = depreciationPlanList.TotalCurrentCharge;
            total32 = depreciationPlanList.TotalCumulativelyCharge;
            total33 = depreciationPlanList.TotalRemainingAmount;


            Assert.AreEqual(total11, total21);
            Assert.AreEqual(total31, total21);
            Assert.AreEqual(total22, total32);
            Assert.AreEqual(total23, total33);
        }

        [TestMethod]
        public void TestCalculateAssetDepreciation()
        {
            Asset asset = new Asset() { Id = 2, AssetName = "Serwer HP", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)1000.21, AmortisedValue = 0, DepreciationTypeId = 1 };
            Dictionary<int, string> monthNames = MocForMonths();
            Dictionary<int, DepreciationType> depreciationTypes = new Dictionary<int, DepreciationType>();
            depreciationTypes.Add(1, new DepreciationType() { Id = 1, Name = "Liniowa 30%", DepreciationRate = (decimal)30 });
            List<DepreciationCharge> depreciationCharges = new List<DepreciationCharge>();
            depreciationCharges.Add(new DepreciationCharge() { Id = 10, No = 1, Month = 1, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)25.01, RemainingAmount = (decimal)975.20, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 11, No = 2, Month = 2, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)50.02, RemainingAmount = (decimal)950.19, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 12, No = 3, Month = 3, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)75.03, RemainingAmount = (decimal)925.18, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 13, No = 4, Month = 4, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)100.04, RemainingAmount = (decimal)900.17, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 14, No = 5, Month = 5, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)125.05, RemainingAmount = (decimal)875.16, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 15, No = 6, Month = 6, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)150.06, RemainingAmount = (decimal)850.15, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 16, No = 7, Month = 7, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)175.07, RemainingAmount = (decimal)825.14, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 17, No = 8, Month = 8, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)200.08, RemainingAmount = (decimal)800.13, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 18, No = 9, Month = 9, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)225.09, RemainingAmount = (decimal)775.12, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 19, No = 10, Month = 10, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)250.10, RemainingAmount = (decimal)750.11, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 20, No = 11, Month = 11, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)275.11, RemainingAmount = (decimal)725.10, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 21, No = 12, Month = 12, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)300.12, RemainingAmount = (decimal)700.09, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 22, No = 13, Month = 1, Year = 2019, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)325.13, RemainingAmount = (decimal)675.08, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 23, No = 14, Month = 2, Year = 2019, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)350.14, RemainingAmount = (decimal)650.07, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 24, No = 15, Month = 3, Year = 2019, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)375.15, RemainingAmount = (decimal)625.06, AssetId = 2 });

            AssetDataSet assetDataSet = new AssetDataSet() { MonthNames = monthNames, DepreciationTypes = depreciationTypes, DepreciationCharges = depreciationCharges };

            Depreciation depreciation = new Depreciation();
            AssetDepreciationPlan assetDepreciationPlan = depreciation.CalculateAssetDepreciationPlan(assetDataSet, asset, false);

            decimal total11 = 0;
            decimal total12 = 0;
            decimal total13 = 0;
            decimal total21 = 0;
            decimal total31 = assetDepreciationPlan.TotalCurrentCharge;
            decimal total32 = assetDepreciationPlan.TotalCumulativelyCharge;
            decimal total33 = assetDepreciationPlan.TotalRemainingAmount;

            foreach (AssetDepreciationYearPlan assetDepreciationYearPlan in assetDepreciationPlan.AssetDepreciationYearPlans)
            {
                foreach(AssetDepreciationMonthPlan assetDepreciationMonthPlan in assetDepreciationYearPlan.AssetDepreciationMonthPlans)
                {
                    total11 += assetDepreciationMonthPlan.CurrentCharge;
                    total12 = assetDepreciationMonthPlan.CumulativelyCharge;
                    total13 = assetDepreciationMonthPlan.RemainingAmount;
                }
                total21 += assetDepreciationYearPlan.TotalYearCharge;
            }

            Assert.AreEqual(total11, total21);
            Assert.AreEqual(total31, total21);

            Assert.AreEqual(total12, total32);
            Assert.AreEqual(total13, total33);
        }

        [TestMethod]
        public void TestCalculateAssetDepreciationPlan()
        {
            Asset asset = new Asset() { Id = 2, AssetName = "Serwer HP", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)1000.21, AmortisedValue = 0, DepreciationTypeId = 1 };
            Dictionary<int, string> monthNames = MocForMonths();
            Dictionary<int, DepreciationType> depreciationTypes = new Dictionary<int, DepreciationType>();
            depreciationTypes.Add(1, new DepreciationType() { Id = 1, Name = "Liniowa 30%", DepreciationRate = (decimal)30 });
            List<DepreciationCharge> depreciationCharges = new List<DepreciationCharge>();
            depreciationCharges.Add(new DepreciationCharge() { Id = 10, No = 1, Month = 1, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)25.01, RemainingAmount = (decimal)975.20, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 11, No = 2, Month = 2, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)50.02, RemainingAmount = (decimal)950.19, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 12, No = 3, Month = 3, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)75.03, RemainingAmount = (decimal)925.18, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 13, No = 4, Month = 4, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)100.04, RemainingAmount = (decimal)900.17, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 14, No = 5, Month = 5, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)125.05, RemainingAmount = (decimal)875.16, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 15, No = 6, Month = 6, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)150.06, RemainingAmount = (decimal)850.15, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 16, No = 7, Month = 7, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)175.07, RemainingAmount = (decimal)825.14, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 17, No = 8, Month = 8, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)200.08, RemainingAmount = (decimal)800.13, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 18, No = 9, Month = 9, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)225.09, RemainingAmount = (decimal)775.12, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 19, No = 10, Month = 10, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)250.10, RemainingAmount = (decimal)750.11, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 20, No = 11, Month = 11, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)275.11, RemainingAmount = (decimal)725.10, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 21, No = 12, Month = 12, Year = 2018, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)300.12, RemainingAmount = (decimal)700.09, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 22, No = 13, Month = 1, Year = 2019, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)325.13, RemainingAmount = (decimal)675.08, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 23, No = 14, Month = 2, Year = 2019, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)350.14, RemainingAmount = (decimal)650.07, AssetId = 2 });
            depreciationCharges.Add(new DepreciationCharge() { Id = 24, No = 15, Month = 3, Year = 2019, CurrentCharge = (decimal)25.01, CumulativelyCharge = (decimal)375.15, RemainingAmount = (decimal)625.06, AssetId = 2 });

            AssetDataSet assetDataSet = new AssetDataSet() { MonthNames = monthNames, DepreciationTypes = depreciationTypes, DepreciationCharges = depreciationCharges };

            Depreciation depreciation = new Depreciation();
            AssetDepreciationPlan assetDepreciationPlan = depreciation.CalculateAssetDepreciationPlan(assetDataSet, asset, true);

            decimal total11 = 0;
            decimal total12 = 0;
            decimal total13 = 0;
            decimal total21 = 0;
            decimal total31 = assetDepreciationPlan.TotalCurrentCharge;
            decimal total32 = assetDepreciationPlan.TotalCumulativelyCharge;
            decimal total33 = assetDepreciationPlan.TotalRemainingAmount;

            foreach (AssetDepreciationYearPlan assetDepreciationYearPlan in assetDepreciationPlan.AssetDepreciationYearPlans)
            {
                foreach (AssetDepreciationMonthPlan assetDepreciationMonthPlan in assetDepreciationYearPlan.AssetDepreciationMonthPlans)
                {
                    total11 += assetDepreciationMonthPlan.CurrentCharge;
                    total12 = assetDepreciationMonthPlan.CumulativelyCharge;
                    total13 = assetDepreciationMonthPlan.RemainingAmount;
                }
                total21 += assetDepreciationYearPlan.TotalYearCharge;
            }

            Assert.AreEqual(total11, total21);
            Assert.AreEqual(total31, total21);

            Assert.AreEqual(total12, total32);
            Assert.AreEqual(total13, total33);
        }

        [TestMethod]
        public void TestEquipment()
        {
            List<Asset> assets = new List<Asset>();

            assets.Add(new Asset() { Id = 2, AssetName = "Serwer HP", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)1000.21, AmortisedValue = 0, DepreciationTypeId = 1 , InventoryNo = "0001", AssetLocationId = 1, AssetTypeId = 5, IsUsed = true});
            assets.Add(new Asset() { Id = 3, AssetName = "Macierz dyskowa 50TB", StartUsingDate = new DateTime(2017, 12, 21), InitialValue = (decimal)25000, AmortisedValue = (decimal)100.04, DepreciationTypeId = 1, InventoryNo = "0002", AssetLocationId = 1, AssetTypeId = 10, IsUsed = true });
            assets.Add(new Asset() { Id = 6, AssetName = "Biurko prezesowej", StartUsingDate = new DateTime(2017, 12, 8), InitialValue = (decimal)10000.00, AmortisedValue = (decimal)2500, DepreciationTypeId = 1, InventoryNo = "0003", AssetLocationId = 4, AssetTypeId = 11, IsUsed = true });
            assets.Add(new Asset() { Id = 9, AssetName = "Biurko pracownika", StartUsingDate = new DateTime(2017, 10, 15), InitialValue = (decimal)500, AmortisedValue = 0, DepreciationTypeId = 2, InventoryNo = "B001", AssetLocationId = 2, AssetTypeId = 6, IsUsed = true });
            assets.Add(new Asset() { Id = 10, AssetName = "Biurko pracownika", StartUsingDate = new DateTime(2017, 10, 15), InitialValue = (decimal)500, AmortisedValue = 0, DepreciationTypeId = 2, InventoryNo = "B002", AssetLocationId = 2, AssetTypeId = 6, IsUsed = true });
            assets.Add(new Asset() { Id = 11, AssetName = "Biurko pracownika", StartUsingDate = new DateTime(2017, 10, 15), InitialValue = (decimal)500, AmortisedValue = 0, DepreciationTypeId = 2, InventoryNo = "B003", AssetLocationId = 5, AssetTypeId = 6, IsUsed = true });
            assets.Add(new Asset() { Id = 12, AssetName = "Biurko pracownika", StartUsingDate = new DateTime(2017, 10, 15), InitialValue = (decimal)500, AmortisedValue = 0, DepreciationTypeId = 2, InventoryNo = "B004", AssetLocationId = 5, AssetTypeId = 6, IsUsed = true });
            assets.Add(new Asset() { Id = 13, AssetName = "Lapto handlowca mobilnego", StartUsingDate = new DateTime(2017, 12, 12), InitialValue = (decimal)3500, AmortisedValue = 0, DepreciationTypeId = 1, InventoryNo = "L001", AssetLocationId = 5, AssetTypeId = 3, IsUsed = true });
            assets.Add(new Asset() { Id = 14, AssetName = "Komputer desctop", StartUsingDate = new DateTime(2017, 10, 1), InitialValue = (decimal)3100, AmortisedValue = 0, DepreciationTypeId = 1, InventoryNo = "L002", AssetLocationId = 5, AssetTypeId = 3, IsUsed = true });



            List<AssetType> assetTypes = new List<AssetType>();

            assetTypes.Add(new AssetType() { Id = 3, LowValueAsset = false, Name = "Zestaw komputerowy" });
            assetTypes.Add(new AssetType() { Id = 4, LowValueAsset = false, Name = "Drukarka" });
            assetTypes.Add(new AssetType() { Id = 5, LowValueAsset = false, Name = "Serwer" });
            assetTypes.Add(new AssetType() { Id = 6, LowValueAsset = true, Name = "Biurko" });
            assetTypes.Add(new AssetType() { Id = 7, LowValueAsset = true, Name = "Krzeslo" });
            assetTypes.Add(new AssetType() { Id = 8, LowValueAsset = false, Name = "Projektor" });
            assetTypes.Add(new AssetType() { Id = 9, LowValueAsset = false, Name = "UPS" });
            assetTypes.Add(new AssetType() { Id = 10, LowValueAsset = false, Name = "Macierz dyskowa" });
            assetTypes.Add(new AssetType() { Id = 11, LowValueAsset = false, Name = "Super biurko prezesowej" });

            Equipment equipment = new Equipment();
            List<EquipmentElement> equipmentElements = equipment.PrepareEquipment(assets, assetTypes);

            int t1 = equipmentElements.Sum(e => e.Quantity);
            decimal t2 = equipmentElements.Sum(e => e.InitialValue);
            decimal t3 = equipmentElements.Sum(e => e.AmortisedValue);

            Assert.AreEqual(t1, 9);
            Assert.AreEqual(t2, (decimal)44600.21);
            Assert.AreEqual(t3, (decimal)2600.04);
        }

    }
}
