using KSerialization;
using TUNING;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.Storages
{
    [SerializationConfig(MemberSerialization.OptIn)]
    ////
    /// based on Refrigerator
    public class FoodCabinet : RefrigeratorConfig
    {
        public new const string ID = "Kelmen-FoodCabinet";

        public const string DisplayName = "Food Cabinet";
        public const string Description = "Based on vanilla Refrigerator but without power.";
        public const string Effect = "Food storage with logic output sending true when full.";

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;

            buildingDef.RequiresPowerInput = false;
            buildingDef.EnergyConsumptionWhenActive = 0;
            buildingDef.ExhaustKilowattsWhenActive = 0;

            buildingDef.InitDef();

            return buildingDef;
        }

        public static void SetDescriptions()
        {
            AddBuildingStrings(ID, DisplayName, Description, Effect);
        }

        public static void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Food, ID);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.Food.BasicFarming, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(0, 255, 0, 255);
        }

    }
}
