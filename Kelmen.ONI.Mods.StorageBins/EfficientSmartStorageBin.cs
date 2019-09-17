using KSerialization;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;
using TUNING;

namespace Kelmen.ONI.Mods.StorageBins
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class EfficientSmartStorageBin : StorageLockerSmartConfig
    {
        public new const string ID = "Kelmen-EfficientSmartStorageBin";

        public const string DisplayName = "Efficient Smart Storage Bin";
        public const string Description = "Work like vanilla smart storage bin, but remove power and heat.";
        public const string Effect = "Send out true signal when defined maximum storage is meet.";

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;
            buildingDef.RequiresPowerInput = false;
            buildingDef.EnergyConsumptionWhenActive = 0;
            buildingDef.ExhaustKilowattsWhenActive = 0;
            //buildingDef.SelfHeatKilowattsWhenActive = 0;

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
            return new Color32(128, 255, 128, 255);
        }

    }
}
