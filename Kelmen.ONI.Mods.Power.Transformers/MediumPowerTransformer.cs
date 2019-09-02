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
        public const string ID = "Kelmen-MediumPowerTransformer";

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

            //int width = 3;
            //int height = 2;
            //string anim = "transformer_kanim";
            //int hitpoints = 30;
            //float construction_time = 30f;
            //float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2; // half materials cost
            //string[] refinedMetals = MATERIALS.REFINED_METALS;
            //float melting_point = 800f;
            //BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
            //EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
            //BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tieR3, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR5, 0.2f);
            //buildingDef.RequiresPowerInput = true;
            //buildingDef.UseWhitePowerOutputConnectorColour = true;
            //buildingDef.PowerInputOffset = new CellOffset(-1, 1);
            //buildingDef.PowerOutputOffset = new CellOffset(1, 0);
            //buildingDef.ElectricalArrowOffset = new CellOffset(1, 0);
            //buildingDef.ExhaustKilowattsWhenActive = 0.25f;
            //buildingDef.SelfHeatKilowattsWhenActive = 1f;
            //buildingDef.ViewMode = OverlayModes.Power.ID;
            //buildingDef.AudioCategory = "Metal";
            //buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            //buildingDef.SelfHeatKilowattsWhenActive = 1f;
            //buildingDef.Entombable = true;
            //buildingDef.GeneratorWattageRating = 2000;
            //buildingDef.GeneratorBaseCapacity = 2000;
            //buildingDef.PermittedRotations = PermittedRotations.FlipH;

            return buildingDef;
        }

        //public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        //{
        //    ((KPrefabID)go.GetComponent<KPrefabID>()).AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
        //    go.AddComponent<RequireInputs>();
        //    BuildingDef def = ((Building)go.GetComponent<Building>()).Def;
        //    Battery battery = go.AddOrGet<Battery>();
        //    battery.powerSortOrder = 1000;
        //    battery.capacity = def.GeneratorWattageRating;
        //    battery.chargeWattage = def.GeneratorWattageRating;
        //    ((Generator)go.AddComponent<PowerTransformer>()).powerDistributionOrder = 9;
        //}

        //public override void DoPostConfigureComplete(GameObject go)
        //{
        //    Object.DestroyImmediate((Object)go.GetComponent<EnergyConsumer>());
        //    go.AddOrGetDef<PoweredActiveController.Def>();
        //}

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
