using KSerialization;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;
using TUNING;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GasValveExactQtyByG : IBuildingConfig
    {
        public const string ID = "Kelmen-GasValveExactQtyByG";

        public const string DisplayName = "Exact Quantity (Gram) Gas Valve";
        public const string Description = "Set the amount to flow through. The amount will be reduced by how many been flow through, till it reached 0 to stop the flow.";
        public const string Effect = "Allows exact amount of gas flow through by Gram.";

        public override BuildingDef CreateBuildingDef()
        {
            int width = 1;
            int height = 2;
            string anim = "valvegas_kanim";
            int hitpoints = 30;
            float construction_time = 10f;
            float[] tieR1_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
            string[] rawMetals = MATERIALS.RAW_METALS;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
            EffectorValues tieR1_2 = NOISE_POLLUTION.NOISY.TIER1;

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tieR1_1, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tieR1_2, 0.2f);

            buildingDef.InputConduitType = ConduitType.Gas;
            buildingDef.OutputConduitType = ConduitType.Gas;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = OverlayModes.GasConduits.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            buildingDef.UtilityOutputOffset = new CellOffset(0, 1);

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            var process = go.AddOrGet<GasValveProcessByG>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Object.DestroyImmediate(go.GetComponent<RequireInputs>());
            Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
            Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());

            go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
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
            AddBuildingToTechnology(GameStrings.Technology.Gases.PressureManagement, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(255, 255, 0, 255);
        }

    }
}
