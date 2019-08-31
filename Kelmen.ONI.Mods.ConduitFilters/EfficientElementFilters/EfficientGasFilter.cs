using KSerialization;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.ConduitFilters.EfficientElementFilters
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class EfficientGasFilter : GasFilterConfig
    {
        new public const string ID = "Kelmen-EfficientGasFilter";

        public const string DisplayName = "Efficient Gas Filter";
        public const string Description = "Intend to work like the vanila gas filter but with power consumption of gas shutoffs and gas pipe element sensor.";
        public const string Effect = "Filter gas by it's element, consume less power.";

        public ConduitPortInfo OutputPort2Info = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));

        #region IBuildingConfig

        public override BuildingDef CreateBuildingDef()
        {
            //var buildingDef = BuildingTemplates.CreateBuildingDef(
            //    id: Id,
            //    width: 3,
            //    height: 1,
            //    anim: "filter_gas_kanim",
            //    hitpoints: 30,
            //    construction_time: 10,
            //    construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
            //    construction_materials: MATERIALS.RAW_METALS,
            //    melting_point: 1600,
            //    build_location_rule: BuildLocationRule.Anywhere,
            //    decor: BUILDINGS.DECOR.PENALTY.TIER0,
            //    noise: NOISE_POLLUTION.NOISY.TIER1);

            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = EfficientGasFilter.ID;
            
            //buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 20; // 1 gas pipe element sensor + 2 gas shutfoff
            //buildingDef.SelfHeatKilowattsWhenActive = 0;
            //buildingDef.ExhaustKilowattsWhenActive = 0;
            //buildingDef.InputConduitType = ConduitType.Gas;
            //buildingDef.OutputConduitType = ConduitType.Gas;
            //buildingDef.Floodable = false;
            //buildingDef.ViewMode = OverlayModes.GasConduits.ID;
            //buildingDef.AudioCategory = "Metal";
            //buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
            //buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
            //buildingDef.PermittedRotations = PermittedRotations.R360;

            buildingDef.InitDef();
            
            return buildingDef;
        }

        //public override void DoPostConfigureComplete(GameObject go)
        //{
        //    go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
        //}

        #endregion

        //public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        //{
        //    base.DoPostConfigurePreview(def, go);
        //    this.AttachPort(go);
        //}

        //void AttachPort(GameObject go)
        //{
        //    go.AddComponent<ConduitSecondaryOutput>().portInfo = this.OutputPort2Info;
        //}

        //public override void DoPostConfigureUnderConstruction(GameObject go)
        //{
        //    base.DoPostConfigureUnderConstruction(go);
        //    this.AttachPort(go);
        //}

        //public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        //{
        //    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        //    go.AddOrGet<Structure>();

        //    go.AddOrGet<ElementFilter>().portInfo = this.OutputPort2Info;
        //    go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
        //}

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
            AddBuildingToTechnology(GameStrings.Technology.Gases.HVAC, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(128, 255, 128, 255);
        }

    }
}
