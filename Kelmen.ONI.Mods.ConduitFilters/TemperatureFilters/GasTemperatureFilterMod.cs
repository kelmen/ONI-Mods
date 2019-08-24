using Harmony;

namespace Kelmen.ONI.Mods.ConduitFilters.TemperatureFilters
{
    public class GasTemperatureFilterMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings
        {
            public static void Prefix()
            {
                GasTemperatureFilter.SetDescriptions();
                GasTemperatureFilter.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        //[HarmonyPatch("Initialize")]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Initialize
        {
            public static void Prefix()
            {
                GasTemperatureFilter.SetTechTree();
            }
        }

        //[HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        [HarmonyPatch(typeof(BuildingComplete))]
        //[HarmonyPatch(nameof(BuildingComplete.OnSpawn))]
        [HarmonyPatch("OnSpawn")]
        public static class ChangeGasTemperatureFilterColor
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (string.Compare(__instance.name, (GasTemperatureFilter.Id + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = GasTemperatureFilter.ChangeColor();
                }
            }
        }
    }
}
