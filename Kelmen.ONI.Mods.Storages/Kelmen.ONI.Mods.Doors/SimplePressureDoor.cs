using KSerialization;
using TUNING;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.Doors
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SimplePressureDoor : PressureDoorConfig
    {
        public new const string ID = "Kelmen-SimplePressureDoor";

        public const string DisplayName = "Simple Mechanized Airlock";
        public const string Description = "Based on vanilla mechanized airlock but without power.";
        public const string Effect = "Airlock.";

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;

            buildingDef.RequiresPowerInput = false;
            buildingDef.EnergyConsumptionWhenActive = 0;

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
            AddBuildingToTechnology(GameStrings.Technology.Gases.Decontamination, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(0, 128, 0, 255);
        }

    }
}
