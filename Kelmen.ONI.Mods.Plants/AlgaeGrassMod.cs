using Harmony;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.CarePackagesUtils;

namespace Kelmen.ONI.Mods.Plants
{
    public class AlgaeGrassMod
    {
        [HarmonyPatch(typeof(EntityConfigManager))]
        [HarmonyPatch("LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities
        {
            public static void Prefix()
            {
                AlgaeGrass.SetDescriptions();
                AlgaeGrass.SetPlantSeedStrings();
            }
        }

        [HarmonyPatch(typeof(Immigration))]
        [HarmonyPatch("ConfigureCarePackages")]
        public static class Immigration_ConfigureCarePackages_Patch
        {
            public static void Postfix(ref Immigration __instance)
            {
                AddCarePackage(ref __instance, AlgaeGrass.SeedId, 1);
            }
        }

    }
}
