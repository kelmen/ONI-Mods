using KSerialization;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;
using TUNING;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GasValveExactQtyByKG : GasValveExactQtyByG
    {
        new public const string ID = "Kelmen-GasValveExactQtyByKG";

        new public const string DisplayName = "Exact Quantity (KiloGram) Gas Valve";

        new public const string Effect = "Allows exact amount of gas flow through by KiloGram.";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;

            buildingDef.InitDef();

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            var process = go.AddOrGet<GasValveProcessByKG>();
        }

        public static new void SetDescriptions()
        {
            AddBuildingStrings(ID, DisplayName, Description, Effect);
        }

        public static new void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Ventilation, ID);
        }

        public static new void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.Gases.PressureManagement, ID);
        }

        public static new Color32 ChangeColor()
        {
            return new Color32(255, 255, 0, 255);
        }

    }
}
