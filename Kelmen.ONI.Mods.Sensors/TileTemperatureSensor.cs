using KSerialization;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;
using TUNING;

namespace Kelmen.ONI.Mods.Sensors
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class TileTemperatureSensor : IBuildingConfig
    {
        public const string ID = "Kelmen-TileTemperatureSensor";

        public const string DisplayName = "Tile Temperature Sensor";
        public const string Description = "Monitor the temperature of tile it built into.";
        public const string Effect = "Send out true signal when defined temperature threshold is meet.";

        public static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0)
            , (string)STRINGS.BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR.LOGIC_PORT
            , (string)STRINGS.BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR.LOGIC_PORT_ACTIVE
            , (string)STRINGS.BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR.LOGIC_PORT_INACTIVE
            , false, false);

        public override BuildingDef CreateBuildingDef()
        {
            int width = 1;
            int height = 1;
            string anim = "switchthermal_kanim";
            int hitpoints = 30;
            float construction_time = 30f;
            float[] tieR0 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            string[] refinedMetals = MATERIALS.REFINED_METALS;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
            EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tieR0, refinedMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
            buildingDef.Overheatable = false;
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.ViewMode = OverlayModes.Logic.ID;
            
            //buildingDef.SceneLayer = Grid.SceneLayer.Building;
            buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;

            buildingDef.ObjectLayer = ObjectLayer.LogicGates;

            //buildingDef.AudioCategory = "Metal";
            //SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_on", TUNING.NOISE_POLLUTION.NOISY.TIER3);
            //SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_off", TUNING.NOISE_POLLUTION.NOISY.TIER3);

            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, TileTemperatureSensor.ID);
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            AttachLogicOutputPort(go);

            var temperatureSensor = go.AddOrGet<TileTemperatureSensorProcess>();
            temperatureSensor.manuallyControlled = false;
            temperatureSensor.minTemp = 0.0f;
            temperatureSensor.maxTemp = 9999f;
        }

        void AttachLogicOutputPort(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, TileTemperatureSensor.OUTPUT_PORT);
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            AttachLogicOutputPort(go);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            AttachLogicOutputPort(go);
        }

        public static void SetDescriptions()
        {
            AddBuildingStrings(ID, DisplayName, Description, Effect);
        }

        public static void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Automation, ID);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.Gases.HVAC, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(85, 85, 85, 255);
        }

    }
}
