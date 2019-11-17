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
    public class SimpleSolidDispenser : IBuildingConfig
    {
        public const string ID = "Kelmen-SimpleSolidDispenser";

        public const string DisplayName = "Simple Items Dispenser";
        public const string Description = "Work like vanilla Automatic Dispenser with all-time True automation flag.";
        public const string Effect = "Just drop item on nearby floor.";

        #region IBuildingConfig

        public override BuildingDef CreateBuildingDef()
        {
            int width = 1;
            int height = 2;
            string anim = "object_dispenser_kanim";
            int hitpoints = 30;
            float construction_time = 10f;
            float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
            string[] allMetals = MATERIALS.ALL_METALS;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
            EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.Overheatable = false;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            //buildingDef.RequiresPowerInput = false;
            buildingDef.PermittedRotations = PermittedRotations.FlipH;
            //buildingDef.EnergyConsumptionWhenActive = 0;
            //buildingDef.ExhaustKilowattsWhenActive = 0;
            SoundEventVolumeCache.instance.AddVolume("ventliquid_kanim", "LiquidVent_squirt", TUNING.NOISE_POLLUTION.NOISY.TIER0);
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<ObjectDispenser>().dropOffset = new CellOffset(1, 0);

            Prioritizable.AddRef(go);

            Storage storage = go.AddOrGet<Storage>();
            storage.showInUI = true;
            storage.allowItemRemoval = false;
            storage.showDescriptor = true;
            storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
            storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
            storage.fetchCategory = Storage.FetchCategory.GeneralStorage;

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
        }

        #endregion

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
