using KSerialization;
using TUNING;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.ConduitFilters.DiseaseFilters
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GasDiseaseFilter : IBuildingConfig
    {
        public const string ID = "Kelmen-GasDiseaseFilter";

        public const string DisplayName = "Gas Disease Filter";
        public const string Description = "Route gas with any disease to 2nd outflow.";
        public const string Effect = Description;

        public ConduitPortInfo OutputPort2Info = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));

        #region IBuildingConfig

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 3,
                height: 1,
                anim: "filter_gas_kanim",
                hitpoints: 30,
                construction_time: 10,
                construction_mass: new float[2] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0], TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0] },
                construction_materials: new string[2] { MATERIALS.REFINED_METAL, MATERIALS.PLASTIC },
                melting_point: 1600,
                build_location_rule: BuildLocationRule.Anywhere,
                decor: BUILDINGS.DECOR.PENALTY.TIER0,
                noise: NOISE_POLLUTION.NOISY.TIER1);

            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 20; // 1 gas pipe germ sensor + 2 gas shutfoff
            buildingDef.SelfHeatKilowattsWhenActive = 0;
            buildingDef.ExhaustKilowattsWhenActive = 0;
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
            go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;

            var process = go.AddOrGet<DiseaseFilterProcess>();
            process.OutputPort2Info = OutputPort2Info;
        }

        #endregion

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            base.DoPostConfigurePreview(def, go);
            this.AttachPort(go);
        }

        void AttachPort(GameObject go)
        {
            go.AddComponent<ConduitSecondaryOutput>().portInfo = this.OutputPort2Info;
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            this.AttachPort(go);
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<Structure>();

            var process = go.AddOrGet<DiseaseFilterProcess>();
            process.OutputPort2Info = this.OutputPort2Info;
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
            AddBuildingToTechnology(GameStrings.Technology.Medicine.PathogenDiagnostics, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(255, 255, 0, 255);
        }
    }
}
