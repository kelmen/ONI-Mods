using KSerialization;
using TUNING;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.ConduitFilters.EfficientElementFilters
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class EfficientGasFilter : GasFilterConfig
    {
        new public const string ID = "Kelmen-EfficientGasFilter";

        public const string DisplayName = "Efficient Gas Filter";
        public const string Description = "Intend to work like the vanila gas filter but with power consumption of gas shutoffs and gas pipe element sensor.";
        public const string Effect = "Filter gas by it's element, consume less power.";

        public ConduitPortInfo OutputPort2Info = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));

        #region IBuildingConfig

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = EfficientGasFilter.ID;

            buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
            buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
            buildingDef.EnergyConsumptionWhenActive = 20; // 1 gas pipe element sensor + 2 gas shutfoff

            buildingDef.InitDef();
            
            return buildingDef;
        }

        #endregion

        public static void SetDescriptions()
        {
            AddBuildingStrings(ID, DisplayName, Description, Effect);
        }

        public static void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Ventilation, ID);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.Gases.HVAC, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(128, 255, 128, 255);
        }

    }
}
