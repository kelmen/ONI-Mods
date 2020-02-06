using KSerialization;
using TUNING;
using UnityEngine;
using System.Collections.Generic;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;


namespace Kelmen.ONI.Mods.ConduitFilters.TemperatureFilters
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SolidTemperatureFilter : IBuildingConfig
    {
        // code based on Caith's Conveyor Filter

        public const string ID = "Kelmen-SolidTemperatureFilter";

        public const string DisplayName = "Solid Material Temperature Filter";
        public const string Description = "Filter solid material by temperature.";
        public const string Effect = Description;

        public readonly ConduitPortInfo OutputPort2Info = new ConduitPortInfo(ConduitType.Solid, new CellOffset(0, 0));

        #region IBuildingConfig

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 3,
                height: 1,
                anim: "utilities_conveyorbridge_kanim",
                hitpoints: 30,
                construction_time: 10,
                construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
                construction_materials: MATERIALS.REFINED_METALS,
                melting_point: 1600,
                build_location_rule: BuildLocationRule.Anywhere,
                decor: BUILDINGS.DECOR.PENALTY.TIER0,
                noise: NOISE_POLLUTION.NOISY.TIER1);

            //buildingDef.ObjectLayer = ObjectLayer.SolidConduit;
            //buildingDef.SceneLayer = Grid.SceneLayer.SolidConduits;
            buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;

            buildingDef.RequiresPowerInput = true;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.EnergyConsumptionWhenActive = 20;
            buildingDef.SelfHeatKilowattsWhenActive = 0;
            buildingDef.ExhaustKilowattsWhenActive = 0;
            
            buildingDef.Floodable = false;
            
            buildingDef.AudioCategory = "Metal";
            
            buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
            buildingDef.InputConduitType = ConduitType.Solid;

            buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
            buildingDef.OutputConduitType = ConduitType.Solid;

            buildingDef.PermittedRotations = PermittedRotations.R360;

            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, ID);

            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            BuildingTemplates.DoPostConfigure(go);
            Prioritizable.AddRef(go);
            go.AddOrGet<EnergyConsumer>();
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            var tagList = new List<Tag>();
            tagList.AddRange(STORAGEFILTERS.NOT_EDIBLE_SOLIDS);
            tagList.AddRange(STORAGEFILTERS.FOOD);

            var storage = go.AddOrGet<Storage>();
            storage.capacityKg = 0f;
            storage.showInUI = true;
            storage.showDescriptor = true;
            storage.storageFilters = tagList;
            storage.allowItemRemoval = false;
            storage.onlyTransferFromLowerPriority = false;

            go.AddOrGet<Automatable>();
            go.AddOrGet<TreeFilterable>();

            var process = go.AddOrGet<TemperatureFilterProcess>();
            process.OutputPort2Info = OutputPort2Info;

            go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
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

            var component = go.GetComponent<Constructable>();
            component.requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        }

        public static void SetDescriptions()
        {
            AddBuildingStrings(ID, DisplayName, Description, Effect);
        }

        public static void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Shipping, ID);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.SolidMaterial.SolidTransport, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(255, 128, 128, 255);
        }
    }
}
