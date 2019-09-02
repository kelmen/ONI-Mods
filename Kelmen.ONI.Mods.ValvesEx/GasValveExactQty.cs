using KSerialization;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GasValveExactQty : GasValveConfig
    {
        new public const string ID = "Kelmen-GasValveExactQty";

        public const string DisplayName = "Exact Quantity Gas Valve";
        public const string Description = "Set the amount to flow through. The amount will be reduced by how many been flow through, till it reached 0 to stop the flow.";
        public const string Effect = "Allows exact amount of gas flow through.";

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            var process = go.AddOrGet<GasValveProcess>();
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
