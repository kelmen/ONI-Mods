using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;
using Object = UnityEngine.Object;

using CaiLib.Utils;
using static CaiLib.Utils.StringUtils;
using static CaiLib.Utils.BuildingUtils;

namespace Kelmen.ONI.Mods.TemperatureFilterPipe
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GasTemperatureFilterCfg : GasFilterConfig
    {
        public const string Id = "GasTemperatureFilter";
        //public const string Anim = "gassplitter_a_kanim";
        //const string AudioCtgy = "Metal";
        //const string AudioSize = "small";

        public const string DisplayName = "Gas Temperature Filter";
        public const string Description = "Intend to work like the vanila gas filter but by temperature instead of gas type.";
        public const string Effect = "Routing gas by it's temperature over your configuration.";

        readonly ConduitPortInfo Port2Info = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(
                id: Id,
                width: 3,
                height: 1,
                anim: "filter_gas_kanim",
                hitpoints: 30,
                construction_time: 10,
                construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
                construction_materials: MATERIALS.RAW_METALS,
                melting_point: 1600,
                build_location_rule: BuildLocationRule.Anywhere,
                decor: BUILDINGS.DECOR.PENALTY.TIER0,
                noise: NOISE_POLLUTION.NOISY.TIER1);

            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 10; // gas pipe therme sensor + gas shutfoff
            buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.InputConduitType = ConduitType.Gas;
            buildingDef.OutputConduitType = ConduitType.Gas;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = OverlayModes.GasConduits.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
            buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
            buildingDef.PermittedRotations = PermittedRotations.R360;

            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            //base.DoPostConfigureComplete(go);
            go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;

            //Object.DestroyImmediate(go.GetComponent<RequireInputs>());
            //Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
            //Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());

            //BuildingTemplates.DoPostConfigure(go);
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            base.DoPostConfigurePreview(def, go);

            AttachPort(go);
        }

        void AttachPort(GameObject go)
        {
            go.AddComponent<ConduitSecondaryOutput>().portInfo = Port2Info;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            //base.ConfigureBuildingTemplate(go, prefab_tag);
            ((KPrefabID)go.GetComponent<KPrefabID>()).AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
            go.AddOrGet<Structure>();
            //go.AddOrGet<ElementFilter>().portInfo = this.Port2Info;
            //go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;


            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            var logic = go.AddOrGet<Process>();
            logic.PipingType = ConduitType.Gas;
            logic.OutputPort2 = Port2Info;

            //var port2 = go.GetComponent<ConduitSecondaryOutput>();
            //logic.OutputPort2 = port2.portInfo;

            var cfgData = go.AddOrGet<CfgData>();
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);

            AttachPort(go);
        }

        public static void SetDescriptions()
        {
            AddBuildingStrings(Id, DisplayName, Description, Effect);
        }

        public static void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Ventilation, Id);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.Gases.HVAC, Id);
            //AddBuildingToTechnology(GameStrings.Technology.Liquids.Filtration, GasPipingCfg.Id);
        }
    }
}
