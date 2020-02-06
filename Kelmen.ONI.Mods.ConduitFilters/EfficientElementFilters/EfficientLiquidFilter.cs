using KSerialization;
using TUNING;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.ConduitFilters.EfficientElementFilters
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class EfficientLiquidFilter : LiquidFilterConfig
    {
        new public const string ID = "Kelmen-EfficientLiquidFilter";

        public const string DisplayName = "Efficient Liquid Filter";
        public const string Description = "Intend to work like the vanila liquid filter but with power consumption of liquid shutoffs and liquid pipe element sensor.";
        public const string Effect = "Filter liquid by it's element, consume less power.";

        public ConduitPortInfo OutputPort2Info = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));

        #region IBuildingConfig

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;

            buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
            buildingDef.EnergyConsumptionWhenActive = 20; // 1 gas pipe element sensor + 2 gas shutfoff
            buildingDef.SelfHeatKilowattsWhenActive = 0;

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
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Plumbing, ID);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.Liquids.LiquidTuning, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(128, 255, 128, 255);
        }

    }
}
