using FixedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixedAssets.Logic
{
    public class Equipment
    {

        public List<EquipmentElement> PrepareEquipment(List<Asset>assets, List<AssetType>assetTypes)
        {
            List<EquipmentElement> equipmentElements = new List<EquipmentElement>();

            foreach (AssetType assetType in assetTypes)
            {
                EquipmentElement equipmentElement = new EquipmentElement();
                equipmentElement.Id = assetType.Id;
                equipmentElement.EquipmentName = assetType.Name;

                List<Asset> equipmentAssets = assets.Where(a => a.AssetTypeId == assetType.Id && a.IsUsed == true).ToList();
                equipmentElement.Quantity = equipmentAssets.Count;
                equipmentElement.InitialValue = equipmentAssets.Sum(a => a.InitialValue);
                equipmentElement.AmortisedValue = equipmentAssets.Sum(a => a.AmortisedValue);
                equipmentElement.RemainingAmount = equipmentAssets.Sum(a => a.InitialValue) - equipmentAssets.Sum(a => a.AmortisedValue);

                equipmentElements.Add(equipmentElement);
            }

            return equipmentElements;
        }


    }
}