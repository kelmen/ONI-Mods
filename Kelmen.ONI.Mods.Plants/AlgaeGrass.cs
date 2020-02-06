using System.Collections.Generic;
using UnityEngine;
using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.Plants
{
    public class AlgaeGrass : IEntityConfig
    {
        // based on SeaLettuceConfig

        public static string ID = "Kelmen.AlgaeGrass";
        public const float WATER_RATE = 0.008333334f;
        public const float FERTILIZATION_RATE = 0.0008333334f;

        public const string DisplayName = "Algae Grass";
        public const string Description = "Underwater plant that grows algae.";
        public const string DomesticateDesc = "Plant it underwater and feed it dirt to grow algae.";

        public static readonly string CropId = SimHashes.Algae.ToString();

        public const string SeedId = "AlgaeGrassSeed";
        public const string SeedName = "Algae Grass Seed";
        public static string SeedDesc = $"The {STRINGS.UI.FormatAsLink("Seed", "PLANTS")} of a {STRINGS.UI.FormatAsLink(DisplayName, ID)}.";

        #region IEntityConfig

        public GameObject CreatePrefab()
        {
            GameObject placedEntity = EntityTemplates.CreatePlacedEntity(ID, DisplayName, Description, 1
                , Assets.GetAnim("sea_lettuce_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, TUNING.DECOR.BONUS.TIER0, new EffectorValues(), SimHashes.Creature, null, 308.15f);
            GameObject template = placedEntity;
            float temperature_lethal_low = 263.15f; // -10C
            float temperature_warning_low = 258.15f; // -15C
            float temperature_warning_high = 383.15f; // 110C
            float temperature_lethal_high = 388.15f; // 115C
            bool pressure_sensitive = false;
            SimHashes[] safe_elements = new SimHashes[4]
            {
                SimHashes.Water,
                SimHashes.DirtyWater,
                SimHashes.SaltWater,
                SimHashes.Brine
            };
            EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, safe_elements, pressure_sensitive, 0, 0.15f
                , CropId, false
                , true, true, true, 2400f);

            EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
            {
                new PlantElementAbsorber.ConsumeInfo()
                {
                    tag = SimHashes.Dirt.CreateTag(),
                    massConsumptionRate = 0.0008333334f
                }
            });

            //var drownMonitor = placedEntity.GetComponent<DrowningMonitor>();
            //drownMonitor.canDrownToDeath = false;
            //drownMonitor.livesUnderWater = true;

            placedEntity.AddOrGet<StandardCropPlant>();
            placedEntity.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
            placedEntity.AddOrGet<LoopingSounds>();
            EntityTemplates.CreateAndRegisterPreviewForPlant(
                EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest
                    , ID + "Seed", DisplayName, Description
                    , Assets.GetAnim("seed_sealettuce_kanim"), "object", 0
                    , new List<Tag>() { TagManager.Create(nameof(SeedName)) }
                    , SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 1
                    , DomesticateDesc
                    , EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, string.Empty, false)
                , ID + "_preview", Assets.GetAnim("sea_lettuce_kanim"), "place", 1, 2);

            var soundInst = SoundEventVolumeCache.instance;
            soundInst.AddVolume("sea_lettuce_kanim", "SeaLettuce_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
            soundInst.AddVolume("sea_lettuce_kanim", "SeaLettuce_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);

            return placedEntity;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }

        #endregion

        public static void SetDescriptions()
        {
            AddPlantStrings(ID, DisplayName, Description, DomesticateDesc);
        }

        public static void SetPlantSeedStrings()
        {
            AddPlantSeedStrings(ID, SeedName, SeedDesc);
        }

    }
}
