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
    /// based on ObjectDispenser
    /// 
    public class SimpleSolidDispenser : ObjectDispenserConfig
    {
        public new const string ID = "Kelmen-SimpleSolidDispenser";

        public const string DisplayName = "Simple Items Dispenser";
        public const string Description = "Work like vanilla Automatic Dispenser but exclude power.";
        public const string Effect = "Drop item on nearby floor when receive true signal.";

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
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Base, ID);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.SolidMaterial.SmartStorage, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(0, 255, 0, 255);
        }

    }
}
