using KSerialization;
using TUNING;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.Pumps
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BoostedGasPump : GasPumpConfig
    {
        new public const string ID = "Kelmen-BoostedGasPump";

        public const string DisplayName = "Boosted Gas Pump";
        public const string Description = "Gas pump that can fully fill (1kg) the gas pipe.";
        public const string Effect = "Suck in 1kg of gas.";

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = BoostedGasPump.ID;

            buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;

            buildingDef.EnergyConsumptionWhenActive = 240f * 2;

            buildingDef.InitDef();

            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            base.DoPostConfigureComplete(go);

            ElementConsumer elementConsumer = go.GetComponent<ElementConsumer>();
            elementConsumer.consumptionRate = 1;
            elementConsumer.consumptionRadius = 3;
        }

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
            AddBuildingToTechnology(GameStrings.Technology.Gases.ImprovedVentilation, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(255, 128, 128, 255);
        }
    }
}
