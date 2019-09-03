using KSerialization;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;
using TUNING;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class LiquidValveExactQtyByKG : LiquidValveExactQtyByG
    {
        new public const string ID = "Kelmen-LiquidValveExactQtyByKG";

        new public const string DisplayName = "Exact Quantity (KG) Liquid Valve";
        
        new public const string Effect = "Allows exact amount of liquid flow through by KiloGram.";

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;
            buildingDef.InitDef();

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            go.AddOrGet<LiquidValveByKGProcess>();
        }

        public static new void SetDescriptions()
        {
            AddBuildingStrings(ID, DisplayName, Description, Effect);
        }
        public static new void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Plumbing, ID);
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
