using Harmony;

namespace Kelmen.ONI.Mods.ConduitFilters.TemperatureFilters
{
    public class LiquidTemperatureFilterMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings
        {
            public static void Prefix()
            {
                LiquidTemperatureFilter.SetDescriptions();
                LiquidTemperatureFilter.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Initialize
        {
            public static void Prefix()
            {
                LiquidTemperatureFilter.SetTechTree();
            }
        }

        [HarmonyPatch(typeof(BuildingComplete))]
        [HarmonyPatch("OnSpawn")]
        public static class ChangeLiquidTemperatureFilterColor
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (string.Compare(__instance.name, (LiquidTemperatureFilter.ID + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = LiquidTemperatureFilter.ChangeColor();
                }
            }
        }

    }
}
