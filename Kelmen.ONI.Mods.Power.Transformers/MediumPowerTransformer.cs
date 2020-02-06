using KSerialization;
using TUNING;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.Power.Transformers
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class MediumPowerTransformer : PowerTransformerConfig
    {
        public new const string ID = "Kelmen-MediumPowerTransformer";

        public const string DisplayName = "Medium Power Transformer";
        public const string Description = "Like vanila large power transformer, but only output 2KW.";
        public const string Effect = "Transfer power output at 2KW";


        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;
            buildingDef.GeneratorWattageRating = 2000;
            buildingDef.GeneratorBaseCapacity = 2000;

            buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;

            buildingDef.InitDef();
            return buildingDef;
        }

        public static void SetDescriptions()
        {
            AddBuildingStrings(ID, DisplayName, Description, Effect);
        }

        public static void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Power, ID);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.Power.LowResistanceConductors, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(128, 255, 128, 255);
        }

    }
}
